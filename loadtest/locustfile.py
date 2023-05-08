from locust import FastHttpUser, task,between
import time, random, uuid

# GUI: 
# http://localhost:8089
# API: 
# https://dtu-api.herogamers.dev/api
# Installation Guide:
# https://docs.locust.io/en/stable/installation.html
class LocustUser(FastHttpUser):
    wait_time = between(5, 15)
    @task
    def get_products(self):
        self.client.get("/products/all/"+str(random.randint(0,1)))
        #time.sleep(random.randint(5,10))

    @task
    def post_order(self):
      
        self.client.post("/orders",
                        json={
        "checkMarketing": True,
        "submitComment": str(uuid.uuid4()),
        "shippingAddress": {
          "firstName": str(uuid.uuid4()),
          "lastName": str(uuid.uuid4()),
          "email": str(uuid.uuid4()),
          "mobileNr": str(uuid.uuid4()),
          "company": str(uuid.uuid4()),
          "vatNr": str(uuid.uuid4()),
          "country": str(uuid.uuid4()),
          "zipCode": str(uuid.uuid4()),
          "city": str(uuid.uuid4()),
          "address1": str(uuid.uuid4()),
          "address2": str(uuid.uuid4())
        },
        "billingAddress": {
          "firstName": str(uuid.uuid4()),
          "lastName": str(uuid.uuid4()),
          "email": str(uuid.uuid4()),
          "mobileNr": str(uuid.uuid4()),
          "company": str(uuid.uuid4()),
          "vatNr": str(uuid.uuid4()),
          "country": str(uuid.uuid4()),
          "zipCode": str(uuid.uuid4()),
          "city": str(uuid.uuid4()),
          "address1": str(uuid.uuid4()),
          "address2": str(uuid.uuid4())
        },
        "orderDetails": [
          {
            "quantity": random.randInt(1,10),
            "giftWrap": True,
            "recurringOrder": False,
            "productId": "the-english-100g",
          }
        ]
      })
        