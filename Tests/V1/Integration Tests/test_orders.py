import httpx
import unittest


order_properties = [
    "id", "source_id", "order_date", "request_date", "reference",
    "reference_extra", "order_status", "notes", "shipping_notes", 
    "picking_notes", "warehouse_id", "ship_to", "bill_to", "shipment_id",
    "total_amount", "total_discount", "total_tax", "total_surcharge", "created_at", "updated_at", "items", "is_deleted"
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


    def test_01_get_orders(self): 
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
            

    def test_02_get_order(self):
        # Stuur de request
        response = self.client.get(url=(self.base_url + "/orders/3"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een object is
        self.assertEqual(type(response.json()), dict)

        # Check dat de object de juiste properties heeft
        self.assertTrue(checkOrder(response.json()))
        self.assertEqual(response.json()["id"], 3)
            

    def test_03_get_order_items(self):
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
    

        
    
    def test_04_post_order(self):
        # order object
        data = {
            "id": 1100000,
            "source_id": 48,
            "order_date": "1991-07-12T08:24:57Z",
            "request_date": "1991-07-16T08:24:57Z",
            "reference": "ORD00011",
            "reference_extra": "Doos als zwembad.",
            "order_status": "Delivered",
            "notes": "Stad zitten hoop.",
            "shipping_notes": "Hal mogelijk nu bot vast gat stom.",
            "picking_notes": "Ik seconde schudden wapen.",
            "warehouse_id": 1,
            "ship_to": None,
            "bill_to": None,
            "shipment_id": 1,
            "total_amount": 9375.06,
            "total_discount": 119.68,
            "total_tax": 294.16,
            "total_surcharge": 46.05,
            "created_at": "1991-07-12T08:24:57Z",
            "updated_at": "1991-07-14T04:24:57Z",
            "items": []
        }
        
        # Stuur de request
        response = self.client.post(url=(self.base_url + "/orders"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 201)


    def test_05_put_order(self):
        data = {
            "id": 2,
            "source_id": 48,
            "order_date": "1991-07-12T08:24:57Z",
            "request_date": "1991-07-16T08:24:57Z",
            "reference": "ORD00011",
            "reference_extra": "Doos als zwembad.",
            "order_status": "Delivered",
            "notes": "Stad zitten hoop.",
            "shipping_notes": "Hal mogelijk nu bot vast gat stom.",
            "picking_notes": "Ik seconde schudden wapen.",
            "warehouse_id": 1,
            "ship_to": None,
            "bill_to": None,
            "shipment_id": 2,
            "total_amount": 9375.06,
            "total_discount": 119.68,
            "total_tax": 294.16,
            "total_surcharge": 46.05,
            "created_at": "1991-07-12T08:24:57Z",
            "updated_at": "1991-07-14T04:24:57Z",
            "items": []
        }
        
        # Stuur de request
        response = self.client.put(url=(self.base_url + "/orders/2"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)


    def test_06_put_order_items(self):
        # Ik weet niet wat dit doet
        
        # Stuur de request
        #response = self.client.put(url=(self.base_url + "/orders/1/items"), headers=self.headers, json=data)
        
        # Check de status code
        #self.assertEqual(response.status_code, 200)
        
        pass

    def test_07_delete_order(self):
        # Stuur de request
        response = self.client.delete(url=(self.base_url + "/orders/1100000"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)

    # Unhappy
    def test_08_post_existing_order(self):
        # Order object
        data = {
            "id": 3,
            "source_id": 48,
            "order_date": "1991-07-12T08:24:57Z",
            "request_date": "1991-07-16T08:24:57Z",
            "reference": "ORD00011",
            "reference_extra": "Doos als zwembad.",
            "order_status": "Delivered",
            "notes": "Stad zitten hoop.",
            "shipping_notes": "Hal mogelijk nu bot vast gat stom.",
            "picking_notes": "Ik seconde schudden wapen.",
            "warehouse_id": 1,
            "ship_to": None,
            "bill_to": None,
            "shipment_id": 3,
            "total_amount": 9375.06,
            "total_discount": 119.68,
            "total_tax": 294.16,
            "total_surcharge": 46.05,
            "created_at": "1991-07-12T08:24:57Z",
            "updated_at": "1991-07-14T04:24:57Z",
            "items": []
        }
        
        # Stuur de request
        response = self.client.post(url=(self.base_url + "/orders"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 400)

    
    
    # Unhappy
    def test_09_post_invalid_order(self):
        # Order object
        data = {
            "id": 11,
            "wrong_property": "wrong"
        }
        
        # Stuur de request
        response = self.client.post(url=(self.base_url + "/orders"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 400)
# to run the file: python -m unittest test_orders.py