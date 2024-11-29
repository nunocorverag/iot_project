import os
import tensorflow as tf
import numpy as np
from flask import Flask, request, jsonify
from werkzeug.utils import secure_filename
from PIL import Image
import io
import requests  # Para hacer peticiones a la API de estado
from datetime import datetime  # Para obtener la fecha actual

# Load the model with the correct path
leaf_classifier_model = tf.keras.models.load_model('/Users/gnunoc/Documents/Projects/TEC/iot_project/ImageAnalyzer/MyAnalyzer/maize_leaf_classifier_v2.keras')

# Configure image size (input size for the model)
image_size = (128, 128)

# Define the directory to save images
save_directory = '/Users/gnunoc/Documents/Projects/TEC/iot_project/ImageAnalyzer/MyAnalyzer/Images'

# Create directory if it doesn't exist
if not os.path.exists(save_directory):
    os.makedirs(save_directory)

# Initialize Flask app
app = Flask(__name__)

# URL de la API de estado
state_url = "http://192.168.228.246:5209/api/Plant/state"

# Función para enviar el estado de la planta a la API
def send_state_to_api(confidence_score):
    fecha_actual = datetime.utcnow().isoformat() + "Z"  # Agregar Z para denotar UTC

    state = "Healthy" if confidence_score <= 0.6 else "Non-Healthy"

    payload = {
        "fecha": fecha_actual,  # Fecha actual en formato ISO 8601
        "state": state,         # Estado de la planta (sano o no sano)
        "plantId": 1            # ID de la planta siempre es 1
    }
    # Si la confianza es mayor a 60%, la planta se considera no sana

    try:
        response = requests.post(state_url, json=payload)
        if response.status_code == 201:
            print("Estado de la planta enviado correctamente.")
        else:
            print(f"Error al enviar el estado: {response.status_code}")
    except requests.exceptions.RequestException as e:
        print(f"Error de conexión al enviar el estado: {e}")

# Route to receive image via POST request
@app.route('/predict', methods=['POST'])
def predict():
    # Check if the 'image' part is in the request
    if 'image' not in request.files:
        return jsonify({'error': 'No file part'}), 400
    
    file = request.files['image']
    
    # Check if the filename is empty
    if file.filename == '':
        return jsonify({'error': 'No selected file'}), 400
    
    # Try to read the image
    try:
        img = Image.open(io.BytesIO(file.read()))
    except Exception as e:
        return jsonify({'error': f'Invalid image file: {str(e)}'}), 400
    
    # Print image information (for debugging purposes)
    print(f"Image received: {file.filename}")
    print(f"Image size: {img.size} (Width x Height)")
    
    # Save the image to the directory
    image_path = os.path.join(save_directory, secure_filename(file.filename))
    img.save(image_path)
    print(f"Image saved at: {image_path}")

    # Predict the plant state using the image
    prediction = predict_image(img)
    
    # Print the prediction result (for debugging purposes)
    print(f"Prediction value: {prediction}")
    confidence_score = prediction[1]

    # Enviar el estado a la API de la planta
    send_state_to_api(confidence_score)

    # Return the prediction as JSON
    return jsonify({
        'confidence_score': float(confidence_score)
    })

# Function to predict the state of a single image
def predict_image(image):
    # Resize the image to the size the model expects
    img = image.resize(image_size)
    
    # Convert image to numpy array and scale pixel values to [0, 1]
    img_array = np.array(img) / 255.0
    
    # Expand dimensions to simulate a batch size of 1
    img_array = np.expand_dims(img_array, axis=0)
    
    # Make prediction
    prediction = leaf_classifier_model.predict(img_array)[0][0]
    
    # Determine category based on the prediction (healthy or non-healthy)
    category = "Healthy" if prediction < 0.5 else "Non-Healthy"
    
    return category, prediction

# Run the Flask app
if __name__ == '__main__':
    # Run the server locally on port 5001
    app.run(host='0.0.0.0', port=5001, debug=True)
