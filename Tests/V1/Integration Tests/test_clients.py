import httpx
import unittest

def checkClients(client):
    json_entry = [
    "id", "name", "address", "city", "zip_code", 
    "province", "country", "contact_name", 
    "contact_phone", "contact_email", "created_at", 
    "updated_at"
    ]     
    for option in json_entry:
        if client.get(option) is None:
            return False
        
    if len(client) != 12:
        return False
    return True

def checkClientId(client):
    if client.get("id") == 1:
        return True
    else:
        return False
    
def checkClientOrder(client):
    json_entry = [
    "id", "source_id", "order_date", "request_date", "reference", 
    "reference_extra", "order_status", "notes", "shipping_notes", 
    "picking_notes", "warehouse_id", "ship_to", "bill_to", 
    "shipment_id", "total_amount", "total_discount", "total_tax", 
    "total_surcharge", "created_at", "updated_at", "items"
    ]

    for option in json_entry:
        if client.get(option) is None:
            return False
    return True

class TestClass(unittest.TestCase):
    def setUp(self):
        self.headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        self.client = httpx
        self.url = "http://localhost:3000/api/v1"


    def test_01_get_clients(self):
        response = self.client.get(url=(self.url + "/clients"), headers=self.headers)

        # Check de status code
        self.assertEqual(response.status_code, 200)

        # Check dat de response een list is
        self.assertEqual(type(response.json()), list)

        # Als de list iets bevat (want een list van 0 objects is inprincipe "legaal")
        if (len(response.json()) > 0):
            self.assertEqual(type(response.json()[0]), dict)
            self.assertTrue(checkClients(response.json()[0]))


    def test_02_get_clients_id(self):
        response_id = self.client.get(url=(self.url + "/clients/2"), headers=self.headers)

        # Check de status code
        self.assertEqual(response_id.status_code, 200)
        
        # Check dat de response een list is
        self.assertEqual(type(response_id.json()), dict)
        
        # Als de list iets bevat (want een list van 0 objects is inprincipe "legaal")

        if (len(response_id.json()) > 0):
            self.assertEqual(type(response_id.json()), dict)
            self.assertTrue(checkClients(response_id.json()))
            self.assertTrue(checkClientId(response_id.json()))
    
    def test_03_get_clients_id_order(self):
        response_order = self.client.get(url=(self.url + "/clients/1/orders"), headers=self.headers)

        # Check de status code
        self.assertEqual(response_order.status_code, 200)
        
        # Check dat de response een list is
        self.assertEqual(type(response_order.json()), list)
        
        # Als de list iets bevat (want een list van 0 objects is inprincipe "legaal")
        if (len(response_order.json()) > 0):
            self.assertEqual(type(response_order.json()), list)
            self.assertTrue(checkClientOrder(response_order.json()[0]))
    
    def test_04_post_client(self):
        data = {
                "id": 10000,
                "name": "jeff",
                "address": "1296 jeff street. 349",
                "city": "jeffview",
                "zip_code": "28301",
                "province": "Colorado",
                "country": "United States",
                "contact_name": "jeff piet",
                "contact_phone": "242.732.3483x2573",
                "contact_email": "jeff@example.net",
                "created_at": "2024-10-01 02:22:53",
                "updated_at": "2024-10-02 20:22:35"
                }
        
        response = self.client.post(url=(self.url + "/clients"), headers=self.headers, json=data)

        self.assertEqual(response.status_code, 201)

    def test_05_put_client(self):
        data = {
                "id": 2,
                "name": "it works!",
                "address": "1296 jeff street. 349",
                "city": "jeffview",
                "zip_code": "28301",
                "province": "Colorado",
                "country": "United States",
                "contact_name": "jeff piet",
                "contact_phone": "242.732.3483x2573",
                "contact_email": "jeff@example.net",
                "created_at": "2024-10-01 02:22:53",
                "updated_at": "2024-10-02 20:22:35"
                }
        
        response = self.client.put(url=(self.url + "/clients/2"), headers=self.headers, json=data)

        self.assertEqual(response.status_code, 200)

    def test_06_delete_client(self):
        # response_create = self.client.post(url=(self.url + "/clients"), headers=self.headers, 
        #     json={
        #         "id": 19820,
        #         "name": "jeff",
        #         "address": "1296 jeff street. 349",
        #         "city": "jeffview",
        #         "zip_code": "28301",
        #         "province": "Colorado",
        #         "country": "United States",
        #         "contact_name": "jeff piet",
        #         "contact_phone": "242.732.3483x2573",
        #         "contact_email": "jeff@example.net",
        #         "created_at": "2024-10-01 02:22:53",
        #         "updated_at": "2024-10-02 20:22:35"
        #         }
        # )
        response = self.client.delete(url=(self.url + "/clients/10000"), headers=self.headers)
        
        self.assertEqual(response.status_code, 200)

    def test_07_Unhappy_post_client_existing_id(self):
        data = {
            "id": 3,
            "name": "Raymond Inc",
            "address": "1296 Daniel Road Apt. 349",
            "city": "Pierceview",
            "zip_code": "28301",
            "province": "Colorado",
            "country": "United States",
            "contact_name": "Bryan Clark",
            "contact_phone": "242.732.3483x2573",
            "contact_email": "robertcharles@example.net",
            "created_at": "2010-04-28 02:22:53",
            "updated_at": "2022-02-09 20:22:35"
        }
        
        response = self.client.post(url=(self.url + "/clients"), headers=self.headers, json=data)

        self.assertEqual(response.status_code, 400)

    def test_08_Unhappy_broken_Object_post_client(self):
        data = {
                "id": 10000,
                "name": "jeff",
                "address": "1296 jeff street. 349",
                "city": 1,
                }
        
        response = self.client.post(url=(self.url + "/clients"), headers=self.headers, json=data)

        self.assertEqual(response.status_code, 400)
