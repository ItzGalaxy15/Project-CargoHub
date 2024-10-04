import httpx
import unittest

def checkShipment(shipment):

    # Check if shipment has right amount of properties
    if len(shipment) != 19:
        return False

    # Check if shipment has the right properties
    if shipment.get("id") == None:
        return False
    if shipment.get("order_id") == None:
        return False
    if shipment.get("source_id") == None:
        return False
    if shipment.get("order_date") == None:
        return False
    if shipment.get("request_date") == None:
        return False
    if shipment.get("shipment_date") == None:
        return False
    if shipment.get("shipment_type") == None:
        return False
    if shipment.get("shipment_status") == None:
        return False
    if shipment.get("notes") == None:
        return False
    if shipment.get("carrier_code") == None:
        return False
    if shipment.get("carrier_description") == None:
        return False
    if shipment.get("service_code") == None:
        return False
    if shipment.get("payment_type") == None:
        return False
    if shipment.get("transfer_mode") == None:
        return False
    if shipment.get("total_package_count") == None:
        return False
    if shipment.get("total_package_weight") == None:
        return False
    if shipment.get("created_at") == None:
        return False
    if shipment.get("updated_at") == None:
        return False
    if shipment.get("items") == None:
        return False

    return True


def checkShipmentItem(shipment_item):
    # Check if shipment item has right amount of properties
    if len(shipment_item) != 2:
        return False

    # Check if shipment has the right properties
    if shipment_item.get("item_id") == None:
        return False
    if shipment_item.get("amount") == None:
        return False
    
    return True


class TestClass(unittest.TestCase):
    def setUp(self):
        self.client = httpx
        self.url = "http://localhost:3000/api/v1"
        self.headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })


    def test_get_shipments(self): 
        # Stuur de request
        response = self.client.get(url=(self.url + "/shipments"), headers=self.headers)
        
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
        response = self.client.get(url=(self.url + "/shipments/1"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een object is
        self.assertEqual(type(response.json()), dict)

        # Check dat de object de juiste properties heeft
        self.assertTrue(checkShipment(response.json()))
        self.assertEqual(response.json()["id"], 1)
    
    
    def test_get_shipment_orders(self):
        # Stuur de request
        response = self.client.get(url=(self.url + "/shipments/1/orders"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een list is
        self.assertEqual(type(response.json()), list)
        
        # Als de list iets bevat (want een list van 0 objects is inprincipe "valid")
        if (len(response.json()) > 0):
            # Check of de list gevuld is met ints
            self.assertEqual(type(response.json()[0]), int)
            
            # Check of de order shipment_id 1 is
            pass
            

    def test_get_shipment_items(self):
        # Stuur de request
        response = self.client.get(url=(self.url + "/shipments/1/items"), headers=self.headers)
        
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
        #! Zou eigenlijk moeten doen via het lezen van de database
        self.assertEqual(response.json(),
            self.client.get(url=(self.url + "/shipments/1"), headers=self.headers).json()["items"])
        
    
    def test_post_shipment(self):
        # Shipment object
        data = {
            "id": 99999,
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
        response = self.client.post(url=(self.url + "/shipments"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 201)
        
        # Check of de shipment in de database zit
        pass
        

    def test_put_shipment(self):
        #! We gebruiken nog geen test-database, dus dit kan niet echt getest worden
        
        data = {
            "id": 99999,
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
        response = self.client.put(url=(self.url + "/shipments/1"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check of de shipment in de database is aangepast
        pass


    def test_put_shipment_orders(self):
        #! We gebruiken nog geen test-database, dus dit kan niet echt getest worden
        # Array van order id's
        data = [1, 2, 3]
        
        # Stuur de request
        response = self.client.put(url=(self.url + "/shipments/1/orders"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check of de bestaande orders in de shipment, die niet meegegeven werden in de data op
        # shipment_id = -1 en shipment_status = "Scheduled" staat
        pass
    
    
    
    def test_put_shipment_items(self):
        # Ik weet niet wat dit doet
        
        # Stuur de request
        #response = self.client.put(url=(self.url + "/shipments/1/items"), headers=self.headers, json=data)
        
        # Check de status code
        #self.assertEqual(response.status_code, 200)
        
        pass
    
    
    def test_put_shipment_commit(self):
        # Doet niks in de code
        
        # Stuur de request
        #response = self.client.put(url=(self.url + "/shipments/1/commit"), headers=self.headers, json=data)
        
        # Check de status code
        #self.assertEqual(response.status_code, 200)
        
        pass


    def test_delete_shipment(self):
         # Stuur de request
        response = self.client.delete(url=(self.url + "/shipments/1"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check of de shipment uit de database is
        pass
