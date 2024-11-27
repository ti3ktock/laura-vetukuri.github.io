import cv2
import numpy as np
from keras.models import load_model
from keras.preprocessing.image import img_to_array
from keras.preprocessing import image

# Load the face classifier and models with error handling
try:
    face_classifier = cv2.CascadeClassifier("./haarcascade_frontalface_default.xml")
    emotion_model = load_model("./model.keras")
    age_model = load_model("./classification_age_model_utk.keras")
    gender_model = load_model("./classification_gender_model_utk.keras")
except Exception as e:
    print(f"Error loading resources: {e}")
    exit()
    
    # Print the model summary to check the expected input shape
print(gender_model.summary())
print(age_model.summary())


emotion_labels = ['Angry', 'Disgust', 'Fear', 'Happy', 'Neutral', 'Sad', 'Surprise']
gender_labels = ['Male', 'Female']

# Initialize the video capture device
cap = cv2.VideoCapture(0)
if not cap.isOpened():
    print("Error: Unable to access the camera.")
    exit()

while True:
    ret, frame = cap.read()
    if not ret:
        print("Failed to capture frame. Exiting...")
        break

    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    gray = cv2.equalizeHist(gray)  # Histogram equalization to improve contrast
    gray = cv2.GaussianBlur(gray, (5, 5), 0)  # Gaussian blur to reduce noise

    faces = face_classifier.detectMultiScale(gray, scaleFactor=1.3, minNeighbors=5)
    
    # Process faces in a batch to improve efficiency
    if len(faces) > 0:
        for (x, y, w, h) in faces:
            roi_gray = gray[y:y+h, x:x+w]
            roi_color = frame[y:y+h, x:x+w]

            # Prepare ROI for emotion prediction
            roi_emotion = cv2.resize(roi_gray, (48, 48), interpolation=cv2.INTER_AREA)
            if np.sum(roi_emotion) != 0:
                roi_emotion = roi_emotion.astype('float32') / 255.0
                roi_emotion = img_to_array(roi_emotion)
                roi_emotion = np.expand_dims(roi_emotion, axis=0)

                # Predict emotion
                emotion_prediction = emotion_model.predict(roi_emotion)
                emotion_label = emotion_labels[emotion_prediction.argmax()]

            # Prepare ROI for age and gender prediction
            roi_color_resized = cv2.resize(roi_color, (224, 224), interpolation=cv2.INTER_AREA)
            roi_color_resized = roi_color_resized.astype('float32') / 255.0
            roi_color_resized = np.expand_dims(roi_color_resized, axis=0)

            
            # Predict age
            age_prediction = age_model.predict(roi_color_resized)
            predicted_age = int(age_prediction[0][0])  # Assuming the output is a single float value

            # Predict gender
            gender_prediction = gender_model.predict(roi_color_resized)
            gender_label = gender_labels[gender_prediction.argmax()]

            # Draws bounding box and labels on the frame
            label_position = (x, y - 10)
            cv2.rectangle(frame, (x, y), (x+w, y+h), (0, 255, 255), 2)
            cv2.putText(frame, f'{emotion_label}, {predicted_age} years, {gender_label}', label_position, cv2.FONT_HERSHEY_SIMPLEX, 0.8, (0, 255, 0), 2)
    else:
        cv2.putText(frame, 'No Face Found', (30, 80), cv2.FONT_HERSHEY_SIMPLEX, 1, (0, 255, 0), 2)

    cv2.imshow('Emotion, Age, and Gender Detector', frame)

    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# Release the video capture and close all OpenCV windows
cap.release()
cv2.destroyAllWindows()
