import cv2
from ultralytics import YOLO
import numpy as np
import torch

# Load the video and store it on cap
cap = cv2.VideoCapture("dogs.mp4")

# This model can detect 80 different classes
device = torch.device('cpu')
model = YOLO("yolov8m.pt")
model.to(device)


while True:
    # ret: Indicates if the frame was successfully read
    # frame: The actual frame as a NumPy array
    ret, frame = cap.read()
    if not ret:
        break


    results = model(frame)
    result = results[0]
    bboxes = np.array(result.boxes.xyxy.cpu(), dtype=int)

    classes = np.array(result.boxes.cls.cpu(), dtype="int")
    for bbox, cls in zip(bboxes, classes):
        (x, y, x2, y2) = bbox
        cv2.rectangle(frame, (x, y), (x2, y2), (0, 0, 255), 2)
        cv2.putText(frame, str(cls), (x, y - 5), cv2.FONT_HERSHEY_PLAIN, 1, (0, 0, 255), 2)

    # Opens a window titled "Img" to display the current frame of the video
    cv2.imshow("Img", frame)

    # If the esc key is pressed, the loop breaks
    key = cv2.waitKey(1)
    if key == 27:
        break

cap.release()
cv2.destroyAllWindows()