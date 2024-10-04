import httpx
import unittest

def checkClients(client):

    # als de warehouse niet die property heeft, return False
    if client.get("id") == None:
        return False
    if client.get("name") == None:
        return False
    if client.get("address") == None:
        return False
    if client.get("city") == None:
        return False
    if client.get("zip_code") == None:
        return False
    if client.get("province") == None:
        return False
    if client.get("country") == None:
        return False
    if client.get("contact_name") == None:
        return False
    if client.get("contact_phone") == None:
        return False
    if client.get("contact_email") == None:
        return False
    if client.get("created_at") == None:
        return False
    if client.get("updated_at") == None:
        return False
    # etc. (elke property van de object)
    # hij zei dat we later met hem kunnen vragen / valideren welke properties een object moet hebben,
    # maar laten we er voor nu maar uitgaan dat inprincipe elke property er moet zijn bij elke object
    # Ik weet niet of deze check moet, aangezien we de "happy path" moeten volgen
    # maar dit zou dan checken dat er niet nog meer properties zijn, die er niet horen te zijn
    if len(client) != 12:
        return False
    # het heeft elke property dus return true
    return True

def checkClientId(client):
    if client.get("id") == 1:
        return True
    else:
        return False
    
def checkClientOrder(client):
    if client.get("id") == None:
        return False
    if client.get("source_id") == None:
        return False
    if client.get("order_date") == None:
        return False
    if client.get("request_date") == None:
        return False
    if client.get("reference") == None:
        return False
    if client.get("reference_extra") == None:
        return False
    if client.get("order_status") == None:
        return False
    if client.get("notes") == None:
        return False
    if client.get("shipping_notes") == None:
        return False
    if client.get("picking_notes") == None:
        return False
    if client.get("warehouse_id") == None:
        return False
    if client.get("ship_to") == None:
        return False
    if client.get("bill_to") == None:
        return False
    if client.get("shipment_id") == None:
        return False
    if client.get("total_amount") == None:
        return False
    if client.get("total_discount") == None:
        return False
    if client.get("total_tax") == None:
        return False
    if client.get("total_surcharge") == None:
        return False
    if client.get("created_at") == None:
        return False
    if client.get("updated_at") == None:
        return False
    if client.get("items") == None:
        return False
    else:
        return True

class TestClass(unittest.TestCase):
    def setUp(self):
        self.headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        self.client = httpx
        self.url = "http://localhost:3000/api/v1"


    def test_get_clients(self):
        
        # Stel de headers op
        
        # Stuur de request
        response = self.client.get(url=(self.url + "/clients"), headers=self.headers)
        response_id = self.client.get(url=(self.url + "/clients/1"), headers=self.headers)
        response_order = self.client.get(url=(self.url + "/clients/1/orders"), headers=self.headers)

        # Check de status code
        self.assertEqual(response.status_code, 200)
        self.assertEqual(response_id.status_code, 200)
        self.assertEqual(response_order.status_code, 200)
        
        # Check dat de response een list is
        self.assertEqual(type(response.json()), list)
        self.assertEqual(type(response_id.json()), dict)
        self.assertEqual(type(response_order.json()), list)
        
        # Als de list iets bevat (want een list van 0 objects is inprincipe "legaal")
        if (len(response.json()) > 0):
            # Check of de object in de list ook echt een "object" (eigenlijk overal een dictionary) is,
            # dus niet dat het een list van ints, strings etc. zijn
            self.assertEqual(type(response.json()[0]), dict)
            # Check dat de object de juiste properties heeft
            self.assertTrue(checkClients(response.json()[0]))

        if (len(response_id.json()) > 0):
            self.assertEqual(type(response_id.json()), dict)
            self.assertTrue(checkClients(response_id.json()))
            self.assertTrue(checkClientId(response_id.json()))

        if (len(response_order.json()) > 0):
            self.assertEqual(type(response_order.json()), list)
            self.assertTrue(checkClientOrder(response_order.json()[0]))

    def test_post_client(self):
        data = {
                "id": 1000000000,
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

        self.assertEqual(response.status_code, 200)

    def test_put_client(self):
        data = {
                "id": 199999999,
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

        self.assertEqual(response.status_code, 200)

    def test_delete_client(self):
        response = self.client.post(url=(self.url + "/clients/199999999"), headers=self.headers)
        
        self.assertEqual(response.status_code, 200)