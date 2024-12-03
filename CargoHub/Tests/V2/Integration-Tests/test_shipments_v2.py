import httpx
import unittest


shipment_properties = [
    "id", "order_id", "source_id", "order_date", "request_date", "shipment_date", 
    "shipment_type", "shipment_status", "notes", "carrier_code", "carrier_description", 
    "service_code", "payment_type", "transfer_mode", "total_package_count", "total_package_weight", 
    "created_at", "updated_at", "items"
]
def checkShipment(shipment):

    # Check if shipment has right amount of properties
    if len(shipment) != len(shipment_properties):
        return False

    # Check if shipment has the right properties
    for property in shipment_properties:
        if property not in shipment:
            return False

    return True

shipment_item_properties = ["item_id", "amount"]
def checkShipmentItem(shipment_item):
    # Check if shipment item has right amount of properties
    if len(shipment_item) != len(shipment_item_properties):
        return False

    # Check if shipment has the right properties
    for property in shipment_item_properties:
        if property not in shipment_item:
            return False
    
    return True


class TestClass(unittest.TestCase):
    def setUp(self):
        self.client = httpx.Client(headers={'API_KEY': 'a1b2c3d4e5'})
        self.url = "http://localhost:3000/api/v2"

    def tearDown(self):
        self.client.close()


    def test_01_get_shipments(self): 
        # Stuur de request
        response = self.client.get(f"{self.url}/shipments")
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een list is
        self.assertEqual(type(response.json()), list)
        
        # Als de list iets bevat (want een list van 0 objects is inprincipe "valid")
        if (len(response.json()) > 0):
            # Check of de object in de list ook echt een "object" (eigenlijk overal een dictionary) is,
            # dus niet dat het een list van ints, strings etc. zijn
            self.assertEqual(type(response.json()[0]), dict)
            
            # Check dat de object de juiste properties heeft
            self.assertTrue(checkShipment(response.json()[0]))
            

    def test_02_get_shipment(self):
        # Stuur de request
        response = self.client.get(f"{self.url}/shipments/1")
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een object is
        self.assertEqual(type(response.json()), dict)

        # Check dat de object de juiste properties heeft
        self.assertTrue(checkShipment(response.json()))
        self.assertEqual(response.json()["id"], 1)
    
    
    def test_03_get_shipment_orders(self):
        # Stuur de request
        response = self.client.get(f"{self.url}/shipments/1/orders")
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een list is
        self.assertEqual(type(response.json()), list)
        
        # Als de list iets bevat (want een list van 0 objects is inprincipe "valid")
        if (len(response.json()) > 0):
            # Check of de list gevuld is met ints
            self.assertEqual(type(response.json()[0]), int)
            
            # Check of de shipment_id van de order 1 is
            order_id = response.json()[0]
            response = self.client.get(f"{self.url}/orders/{order_id}")
            self.assertEqual(response.json()["shipment_id"], 1)
            

    def test_04_get_shipment_items(self):
        # Stuur de request
        response = self.client.get(f"{self.url}/shipments/1/items")
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een list is
        self.assertEqual(type(response.json()), list)
        
        # Als de list iets bevat (want een list van 0 objects is inprincipe "valid")
        if (len(response.json()) > 0):
            # Check of de object in de list ook echt een "object" (eigenlijk overal een dictionary) is,
            # dus niet dat het een list van ints, strings etc. zijn
            self.assertEqual(type(response.json()[0]), dict)
            
            # Check of de item de juiste properties heeft
            self.assertTrue(checkShipmentItem(response.json()[0]))
    
        # Check of de items ook de items in de shipment zijn
        # Zou eigenlijk moeten doen via het lezen van de database
        self.assertEqual(response.json(),
            self.client.get(f"{self.url}/shipments/1").json()["items"])
        
    
    def test_05_post_shipment(self):
        # Shipment object
        data = {
            "id": 17,
            "order_id": 5,
            "source_id": 50,
            "order_date": "2025-02-03",
            "request_date": "2024-10-19",
            "shipment_date": "2025-01-12",
            "shipment_type": "S",
            "shipment_status": "Pending",
            "notes": "",
            "carrier_code": "PostNL",
            "carrier_description": "",
            "service_code": "Fastest",
            "payment_type": "Manual",
            "transfer_mode": "Ground",
            "total_package_count": 31,
            "total_package_weight": 594.42,
            "created_at": "",
            "updated_at": "",
            "items": []
        }
        
        # Stuur de request
        response = self.client.post(f"{self.url}/shipments", json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 201)
    

    def test_06_put_shipment(self):
        data = {
        "id": 2,
        "order_id": 2,
        "source_id": 9,
        "order_date": "1983-11-28",
        "request_date": "1983-11-30",
        "shipment_date": "1983-12-02",
        "shipment_type": "I",
        "shipment_status": "Transit",
        "notes": "Wit duur fijn vlieg.",
        "carrier_code": "PostNL",
        "carrier_description": "Royal Dutch Post and Parcel Service",
        "service_code": "TwoDay",
        "payment_type": "Automatic",
        "transfer_mode": "Ground",
        "total_package_count": 56,
        "total_package_weight": 42.25,
        "created_at": "1983-11-29T11:12:17Z",
        "updated_at": "1983-11-30T13:12:17Z",
        "items": []
        }
        
        # Stuur de request
        response = self.client.put(f"{self.url}/shipments/2", json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        

    def test_07_put_shipment_orders(self):
        #! We gebruiken nog geen test-database, dus dit kan niet echt getest worden
        # Array van order id's
        data = [4]
        
        # Stuur de request
        response = self.client.put(f"{self.url}/shipments/3/orders", json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check of de shipment aangepast is
        response = self.client.get(f"{self.url}/shipments/3/orders")
        self.assertEqual(response.json(), data)
        
        # Check of de bestaande order in de shipment, die niet meegegeven is in de data op
        # shipment_id = -1 en order_status = "Scheduled" staat
        response = self.client.get(f"{self.url}/orders/3")
        self.assertEqual(response.json()["shipment_id"], -1)
        self.assertEqual(response.json()["order_status"], "Scheduled")
        
        # Check of de bestaande order, die wel meegegeven is in de data op
        # shipment_id = 3 en order_status = "Packed" staat
        response = self.client.get(f"{self.url}/orders/4")
        self.assertEqual(response.json()["shipment_id"], 3)
        self.assertEqual(response.json()["order_status"], "Packed")
    
    
    
    def test_08_put_shipment_items(self):
        # Ik weet niet wat dit doet
        
        # Stuur de request
        # response = self.client.put(f"{self.url}/shipments/1/items", json=data)
        
        # Check de status code
        #self.assertEqual(response.status_code, 200)
        
        pass
    
    
    def test_09_put_shipment_commit(self):
        # Doet niks in de code
        
        # Stuur de request
        # response = self.client.put(f"{self.url}/shipments/1/commit", json=data)
        
        # Check de status code
        #self.assertEqual(response.status_code, 200)
        
        pass


    def test_10_delete_shipment(self):
        # Stuur de request
        response = self.client.delete(f"{self.url}/shipments/17")
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        
    # Unhappy
    def test_11_post_existing_shipment(self):
        # Shipment object
        data = {
            "id": 2,
            "order_id": 5,
            "source_id": 50,
            "order_date": "2025-02-03",
            "request_date": "2024-10-19",
            "shipment_date": "2025-01-12",
            "shipment_type": "S",
            "shipment_status": "Pending",
            "notes": "",
            "carrier_code": "PostNL",
            "carrier_description": "",
            "service_code": "Fastest",
            "payment_type": "Manual",
            "transfer_mode": "Ground",
            "total_package_count": 31,
            "total_package_weight": 594.42,
            "created_at": "",
            "updated_at": "",
            "items": []
        }
        
        # Stuur de request
        response = self.client.post(f"{self.url}/shipments", json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 400)
    

    
    
    # Unhappy
    def test_12_post_invalid_shipment(self):
        # Shipment object
        data = {
            "id": 11,
            "wrong_property": "wrong"
        }
        
        # Stuur de request
        response = self.client.post(f"{self.url}/shipments", json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 400)
        
        # # Check dat de foute shipment niet in de database zit
        # response = self.client.get(f"{self.url}/shipments/11")
        # self.assertEqual(response.status_code, 404)
#  python -m unittest test_shipments.py