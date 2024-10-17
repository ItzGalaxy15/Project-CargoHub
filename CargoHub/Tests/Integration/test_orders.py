import httpx
import unittest


order_properties = [
    "id", "source_id", "order_date", "request_date", "reference",
    "reference_extra", "order_status", "notes", "shipping_notes", 
    "picking_notes", "warehouse_id", "ship_to", "bill_to", "shipment_id",
    "total_amount", "total_discount", "total_tax", "total_surcharge", "created_at", "updated_at", "items"
]
def checkOrder(order):
    # Check if order has right amount of properties
    if len(order) != len(order_properties):
        return False

    # Check if order has the right properties
    for property in order_properties:
        if property not in order:
            return False

    return True

order_item_properties = ["item_id", "amount"]
def checkOrderItem(order_item):
    # Check if order item has right amount of properties
    if len(order_item) != len(order_item_properties):
        return False

    # Check if order has the right properties
    for property in order_item_properties:
        if property not in order_item:
            return False
    
    return True


class TestClass(unittest.TestCase):
    
    def setUp(self):
        self.client = httpx
        self.base_url = "http://localhost:3000/api/v1"
        self.headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })


    def test_get_orders(self): 
        # Stuur de request
        response = self.client.get(url=(self.base_url + "/orders"), headers=self.headers)
        
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
            self.assertTrue(checkOrder(response.json()[0]))
            

    def test_get_order(self):
        # Stuur de request
        response = self.client.get(url=(self.base_url + "/orders/1"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een object is
        self.assertEqual(type(response.json()), dict)

        # Check dat de object de juiste properties heeft
        self.assertTrue(checkOrder(response.json()))
        self.assertEqual(response.json()["id"], 1)
            

    def test_get_order_items(self):
        # Stuur de request
        response = self.client.get(url=(self.base_url + "/orders/1/items"), headers=self.headers)
        
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
            self.assertTrue(checkOrderItem(response.json()[0]))
    
        # Check of de items ook de items in de order zijn
        # Zou eigenlijk moeten doen via het lezen van de database
        self.assertEqual(response.json(),
            self.client.get(url=(self.base_url + "/orders/1"), headers=self.headers).json()["items"])
        
    
    def test_post_order(self):
        # order object
        data = {
            "id": 6,
            "source_id": None,
            "order_date": None,
            "request_date": None,
            "reference": None,
            "reference_extra": None,
            "order_status": None,
            "notes": None,
            "shipping_notes": None,
            "picking_notes": None,
            "warehouse_id": None,
            "ship_to": None,
            "bill_to": None,
            "shipment_id": None,
            "total_amount": None,
            "total_discount": None,
            "total_tax": None,
            "total_surcharge": None,
            "created_at": None,
            "updated_at": None,
            "items": []
        }
        
        # Stuur de request
        response = self.client.post(url=(self.base_url + "/orders"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 201)
        
        # Check of de order in de database zit
        response = self.client.get(url=(self.base_url + "/orders/6"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(response.json()["id"], 6)
    
    
    # Unhappy (werkt nu nog niet)
    def test_post_existing_order(self):
        # Order object
        data = {
            "id": 7,
            "source_id": None,
            "order_date": None,
            "request_date": None,
            "reference": None,
            "reference_extra": None,
            "order_status": None,
            "notes": "Wrong",
            "shipping_notes": None,
            "picking_notes": None,
            "warehouse_id": None,
            "ship_to": None,
            "bill_to": None,
            "shipment_id": None,
            "total_amount": None,
            "total_discount": None,
            "total_tax": None,
            "total_surcharge": None,
            "created_at": None,
            "updated_at": None,
            "items": []
        }
        
        # Stuur de request
        response = self.client.post(url=(self.base_url + "/orders"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 400)
        
        # Check dat de order niet de bestaande object heeft overgenomen database zit
        response = self.client.get(url=(self.base_url + "/orders/7"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertNotEqual(response.json()["notes"], "Wrong")
    
    
    # Unhappy (werkt nu nog niet)
    def test_post_invalid_order(self):
        # Order object
        data = {
            "id": 11,
            "wrong_property": "wrong"
        }
        
        # Stuur de request
        response = self.client.post(url=(self.base_url + "/orders"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 400)
        
        # Check dat de foute order niet in de database zit
        response = self.client.get(url=(self.base_url + "/orders/11"), headers=self.headers)
        self.assertEqual(response.status_code, 500)
    

    def test_put_order(self):
        data = {
            "id": 2,
            "source_id": None,
            "order_date": None,
            "request_date": None,
            "reference": None,
            "reference_extra": None,
            "order_status": None,
            "notes": "This order has been modified",
            "shipping_notes": None,
            "picking_notes": None,
            "warehouse_id": None,
            "ship_to": None,
            "bill_to": None,
            "shipment_id": None,
            "total_amount": None,
            "total_discount": None,
            "total_tax": None,
            "total_surcharge": None,
            "created_at": None,
            "updated_at": None,
            "items": []
        }
        
        # Stuur de request
        response = self.client.put(url=(self.base_url + "/orders/2"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check of de order in de database is aangepast
        response = self.client.get(url=(self.base_url + "/orders/2"), headers=self.headers)
        self.assertEqual(response.json()["notes"], "This order has been modified")


    def test_put_order_items(self):
        # Ik weet niet wat dit doet
        
        # Stuur de request
        #response = self.client.put(url=(self.base_url + "/orders/1/items"), headers=self.headers, json=data)
        
        # Check de status code
        #self.assertEqual(response.status_code, 200)
        
        pass


    def test_delete_order(self):
        # Stuur de request
        response = self.client.delete(url=(self.base_url + "/orders/5"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check of de order uit de database is
        response = self.client.get(url=(self.base_url + "/orders/5"), headers=self.headers)
        self.assertEqual(response.json(), None)
