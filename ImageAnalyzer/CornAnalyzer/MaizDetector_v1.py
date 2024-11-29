from sklearn.utils.class_weight import compute_class_weight
import numpy as np
from tensorflow.keras import regularizers  # Importar regularizers
from tensorflow.keras.preprocessing.image import ImageDataGenerator
from tensorflow.keras import layers, models

# Preprocesamiento y carga de datos
image_size = (128, 128)  # Tamaño de las imágenes
batch_size = 32

# Generadores de datos
train_datagen = ImageDataGenerator(
    rescale=1.0 / 255,
    validation_split=0.2  # Separar un 20% para validación
)

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

# Construir el modelo
model = models.Sequential([
    layers.Conv2D(32, (3, 3), activation='relu', kernel_regularizer=regularizers.l2(0.01), input_shape=(128, 128, 3)),
    layers.MaxPooling2D((2, 2)),
    layers.Conv2D(64, (3, 3), activation='relu', kernel_regularizer=regularizers.l2(0.01)),
    layers.MaxPooling2D((2, 2)),
    layers.Flatten(),
    layers.Dense(64, activation='relu', kernel_regularizer=regularizers.l2(0.01)),
    layers.Dropout(0.3),
    layers.Dense(1, activation='sigmoid')
])

# Compilar el modelo
model.compile(optimizer='adam',
              loss='binary_crossentropy',
              metrics=['accuracy'])

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

# Entrenar el modelo
model.fit(
    train_generator,
    epochs=10,
    validation_data=validation_generator,
    class_weight=class_weight_dict,
    verbose=1
)

# Guardar el modelo entrenado
model.save("maize_leaf_classifier.h5")
