from sklearn.utils.class_weight import compute_class_weight
import numpy as np
from tensorflow.keras import regularizers
from tensorflow.keras.preprocessing.image import ImageDataGenerator
from tensorflow.keras import layers, models
import tensorflow as tf

# Preprocesamiento y carga de datos
image_size = (128, 128)  # Tamaño de las imágenes
batch_size = 32

# Generadores de datos con aumentación
train_datagen = ImageDataGenerator(
    rescale=1.0 / 255,
    validation_split=0.2,  # Separar un 20% para validación
    rotation_range=30,  # Rotación aleatoria
    width_shift_range=0.2,  # Desplazamiento horizontal
    height_shift_range=0.2,  # Desplazamiento vertical
    shear_range=0.2,  # Corte aleatorio
    zoom_range=0.2,  # Zoom aleatorio
    horizontal_flip=True,  # Voltear horizontalmente
    fill_mode="nearest"  # Rellenar píxeles faltantes
)

# Generadores de datos
train_generator = train_datagen.flow_from_directory(
    new_dataset_path,
    target_size=image_size,
    batch_size=batch_size,
    class_mode="binary",  # Clasificación binaria
    subset="training"
)

validation_generator = train_datagen.flow_from_directory(
    new_dataset_path,
    target_size=image_size,
    batch_size=batch_size,
    class_mode="binary",
    subset="validation"
)

# Construir el modelo con MobileNetV2 preentrenado
base_model = tf.keras.applications.MobileNetV2(
    weights="imagenet",
    include_top=False,
    input_shape=(128, 128, 3)
)
base_model.trainable = False  # Congelar las capas del modelo base

model = models.Sequential([
    base_model,
    layers.GlobalAveragePooling2D(),
    layers.Dense(64, activation='relu', kernel_regularizer=regularizers.l2(0.01)),
    layers.Dropout(0.3),
    layers.Dense(1, activation='sigmoid')
])

# Compilar el modelo
model.compile(
    optimizer='adam',
    loss='binary_crossentropy',
    metrics=['accuracy', tf.keras.metrics.AUC(name="auc")]
)

# Obtén las etiquetas de las clases en el conjunto de entrenamiento
labels = train_generator.classes  # Etiquetas (0 o 1) de las imágenes
class_indices = train_generator.class_indices  # Mapeo de clases a índices

# Calcula los pesos para las clases
class_weights = compute_class_weight(
    class_weight='balanced',
    classes=np.unique(labels),
    y=labels
)

# Convierte a un diccionario
class_weight_dict = dict(enumerate(class_weights))
print("Pesos calculados:", class_weight_dict)

# Callbacks para evitar sobreajuste y guardar el mejor modelo
callbacks = [
    tf.keras.callbacks.EarlyStopping(patience=5, restore_best_weights=True),
    tf.keras.callbacks.ModelCheckpoint("maize_leaf_classifier.keras", save_best_only=True)
]

# Entrenar el modelo
model.fit(
    train_generator,
    epochs=20,  # Aumentamos las épocas para más entrenamiento
    validation_data=validation_generator,
    class_weight=class_weight_dict,
    callbacks=callbacks,
    verbose=1
)

# Guardar el modelo final entrenado como .h5
model.save("maize_leaf_classifier_final.h5")

import tensorflow as tf
import os

# Cargar el modelo .keras
model = tf.keras.models.load_model("maize_leaf_classifier.keras")

# Configuración del tamaño de la imagen
image_size = (128, 128)

# Función para predecir una sola imagen
def predict_image(image_path):
    img = tf.keras.utils.load_img(image_path, target_size=image_size)  # Cargar la imagen
    img_array = tf.keras.utils.img_to_array(img) / 255.0  # Escalar los valores de píxeles
    img_array = tf.expand_dims(img_array, axis=0)  # Añadir dimensión para lote
    prediction = model.predict(img_array)[0][0]  # Hacer predicción
    category = "Healthy" if prediction < 0.5 else "Non-Healthy"  # Clasificar
    return category, prediction

# Iterar sobre todas las imágenes en la carpeta "predictions"
predictions_folder = "predictions"
for image_name in os.listdir(predictions_folder):
    image_path = os.path.join(predictions_folder, image_name)
    if os.path.isfile(image_path) and image_name.lower().endswith(('.png', '.jpg', '.jpeg')):
        category, prediction = predict_image(image_path)
        print(f"Imagen: {image_name}, Categoría: {category}, Predicción cruda: {prediction:.4f}")
