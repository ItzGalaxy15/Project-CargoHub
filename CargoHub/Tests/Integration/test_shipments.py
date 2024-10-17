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
        self.client = httpx
        self.base_url = "http://localhost:3000/api/v1"
        self.headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })


    def test_get_shipments(self): 
        # Stuur de request
        response = self.client.get(url=(self.base_url + "/shipments"), headers=self.headers)
        
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
            

    def test_get_shipment(self):
        # Stuur de request
        response = self.client.get(url=(self.base_url + "/shipments/1"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een object is
        self.assertEqual(type(response.json()), dict)

        # Check dat de object de juiste properties heeft
        self.assertTrue(checkShipment(response.json()))
        self.assertEqual(response.json()["id"], 1)
    
    
    def test_get_shipment_orders(self):
        # Stuur de request
        response = self.client.get(url=(self.base_url + "/shipments/1/orders"), headers=self.headers)
        
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
            response = self.client.get(url=(self.base_url + f"/orders/{order_id}"), headers=self.headers)
            self.assertEqual(response.json()["shipment_id"], 1)
            

    def test_get_shipment_items(self):
        # Stuur de request
        response = self.client.get(url=(self.base_url + "/shipments/1/items"), headers=self.headers)
        
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
            self.client.get(url=(self.base_url + "/shipments/1"), headers=self.headers).json()["items"])
        
    
    def test_post_shipment(self):
        # Shipment object
        data = {
            "id": 6,
            "order_id": None,
            "source_id": None,
            "order_date": None,
            "request_date": None,
            "shipment_date": None,
            "shipment_type": None,
            "shipment_status": None,
            "notes": None,
            "carrier_code": None,
            "carrier_description": None,
            "service_code": None,
            "payment_type": None,
            "transfer_mode": None,
            "total_package_count": None,
            "total_package_weight": None,
            "created_at": None,
            "updated_at": None,
            "items": []
        }
        
        # Stuur de request
        response = self.client.post(url=(self.base_url + "/shipments"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 201)
        
        # Check of de shipment in de database zit
        response = self.client.get(url=(self.base_url + "/shipments/6"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(response.json()["id"], 6)
    
    
    # Unhappy (werkt nu nog niet)
    def test_post_existing_shipment(self):
        # Supplier object
        data = {
            "id": 7,
            "order_id": None,
            "source_id": None,
            "order_date": None,
            "request_date": None,
            "shipment_date": None,
            "shipment_type": None,
            "shipment_status": None,
            "notes": "Wrong",
            "carrier_code": None,
            "carrier_description": None,
            "service_code": None,
            "payment_type": None,
            "transfer_mode": None,
            "total_package_count": None,
            "total_package_weight": None,
            "created_at": None,
            "updated_at": None,
            "items": []
        }
        
        # Stuur de request
        response = self.client.post(url=(self.base_url + "/shipments"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 400)
        
        # Check dat de supplier niet de bestaande object heeft overgenomen database zit
        response = self.client.get(url=(self.base_url + "/shipments/7"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertNotEqual(response.json()["notes"], "Wrong")
    
    
    # Unhappy (werkt nu nog niet)
    def test_post_invalid_shipment(self):
        # Supplier object
        data = {
            "id": 11,
            "wrong_property": "wrong"
        }
        
        # Stuur de request
        response = self.client.post(url=(self.base_url + "/shipments"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 400)
        
        # Check dat de foute supplier niet in de database zit
        response = self.client.get(url=(self.base_url + "/shipments/11"), headers=self.headers)
        self.assertEqual(response.status_code, 500)
    

    def test_put_shipment(self):
        data = {
            "id": 2,
            "order_id": None,
            "source_id": None,
            "order_date": None,
            "request_date": None,
            "shipment_date": None,
            "shipment_type": None,
            "shipment_status": None,
            "notes": "This shipment has been modified",
            "carrier_code": None,
            "carrier_description": None,
            "service_code": None,
            "payment_type": None,
            "transfer_mode": None,
            "total_package_count": None,
            "total_package_weight": None,
            "created_at": None,
            "updated_at": None,
            "items": []
        }
        
        # Stuur de request
        response = self.client.put(url=(self.base_url + "/shipments/2"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check of de shipment in de database is aangepast
        response = self.client.get(url=(self.base_url + "/shipments/2"), headers=self.headers)
        self.assertEqual(response.json()["notes"], "This shipment has been modified")


    def test_put_shipment_orders(self):
        #! We gebruiken nog geen test-database, dus dit kan niet echt getest worden
        # Array van order id's
        data = [4]
        
        # Stuur de request
        response = self.client.put(url=(self.base_url + "/shipments/3/orders"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check of de shipment aangepast is
        response = self.client.get(url=(self.base_url + "/shipments/3/orders"), headers=self.headers)
        self.assertEqual(response.json(), data)
        
        # Check of de bestaande order in de shipment, die niet meegegeven is in de data op
        # shipment_id = -1 en order_status = "Scheduled" staat
        response = self.client.get(url=(self.base_url + f"/orders/3"), headers=self.headers)
        self.assertEqual(response.json()["shipment_id"], -1)
        self.assertEqual(response.json()["order_status"], "Scheduled")
        
        # Check of de bestaande order, die wel meegegeven is in de data op
        # shipment_id = 3 en order_status = "Packed" staat
        response = self.client.get(url=(self.base_url + f"/orders/4"), headers=self.headers)
        self.assertEqual(response.json()["shipment_id"], 3)
        self.assertEqual(response.json()["order_status"], "Packed")
    
    
    
    def test_put_shipment_items(self):
        # Ik weet niet wat dit doet
        
        # Stuur de request
        #response = self.client.put(url=(self.base_url + "/shipments/1/items"), headers=self.headers, json=data)
        
        # Check de status code
        #self.assertEqual(response.status_code, 200)
        
        pass
    
    
    def test_put_shipment_commit(self):
        # Doet niks in de code
        
        # Stuur de request
        #response = self.client.put(url=(self.base_url + "/shipments/1/commit"), headers=self.headers, json=data)
        
        # Check de status code
        #self.assertEqual(response.status_code, 200)
        
        pass


    def test_delete_shipment(self):
        # Stuur de request
        response = self.client.delete(url=(self.base_url + "/shipments/5"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check of de shipment uit de database is
        response = self.client.get(url=(self.base_url + "/shipments/5"), headers=self.headers)
        self.assertEqual(response.json(), None)
