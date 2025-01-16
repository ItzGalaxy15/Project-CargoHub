import httpx
import unittest

def checkWarehouse(warehouse):

    if len(warehouse) != 12:
        return False

    # po zei dat we later met hem kunnen vragen / valideren welke properties een object moet hebben,
    # maar laten we er voor nu maar uitgaan dat inprincipe elke property er moet zijn bij elke object

    # als de warehouse niet die property heeft, return False
    if warehouse.get("id") == None:
        return False
    if warehouse.get("code") == None:
        return False
    if warehouse.get("name") == None:
        return False
    if warehouse.get("address") == None:
        return False
    if warehouse.get("zip") == None:
        return False
    if warehouse.get("city") == None:
        return False
    if warehouse.get("province") == None:
        return False
    if warehouse.get("country") == None:
        return False
    if warehouse.get("contact") == None:
        return False
    if warehouse.get("created_at") == None:
        return False
    if warehouse.get("updated_at") == None:
        return False

    # het heeft elke property dus return true
    return True



def checkLocation(location):

    # als de location niet die property heeft, return False
    if location.get("id") == None:
        return False
    if location.get("warehouse_id") == None:
        return False
    if location.get("code") == None:
        return False
    if location.get("name") == None:
        return False
    if location.get("created_at") == None:
        return False
    if location.get("updated_at") == None:
        return False


    if len(location) != 7:
        return False

    # het heeft elke property dus return true
    return True


class TestClass(unittest.TestCase):
    def setUp(self):
        self.client = httpx.Client()
        self.url = "http://localhost:3000/api/v1"
        self.headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })


    def test_01_get_warehouses(self):
        
        # Stuur de request
        response = self.client.get(url=(self.url + "/warehouses"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een list is
        self.assertEqual(type(response.json()), list)
        
        # Als de list iets bevat (want een list van 0 objects is inprincipe "legaal")
        if (len(response.json()) > 0):
            # Check of de object in de list ook echt een "object" (eigenlijk overal een dictionary) is,
            # dus niet dat het een list van ints, strings etc. zijn
            self.assertEqual(type(response.json()[0]), dict)
            
            # Check dat de object de juiste properties heeft
            # self.assertTrue(checkWarehouse(response.json()[0]))

            # Check dat elk warehouse-object de juiste eigenschappen heeft
            self.assertTrue(
                all(
                    checkWarehouse(warehouse)
                    for warehouse in response.json()
                )
            )


    def test_02_get_warehouse_id(self):
        # Stuur de request
        response = self.client.get(url=(self.url + "/warehouses/1"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een dictionary is (representatief voor een enkel warehouse object)
        self.assertEqual(type(response.json()), dict)
        
        # Check dat het warehouse object de juiste properties heeft
        self.assertTrue(checkWarehouse(response.json()))


    def test_03_get_warehouse_id_locations(self):
        # Stuur de request
        response = self.client.get(url=(self.url + "/warehouses/1/locations"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een list is (representatief voor een lijst met locaties)
        self.assertEqual(type(response.json()), list)
        
        # Als de lijst iets bevat, controleer dan het eerste object in de lijst
        if len(response.json()) > 0:
            # Check dat elk object in de lijst een dictionary is
            self.assertEqual(type(response.json()[0]), dict)
            
            # Check dat elk locatie-object de juiste eigenschappen heeft
            self.assertTrue(
                all(
                    checkLocation(location)
                    for location in response.json()
                )
            )

    # deze voegt een nieuwe warehouse object
    def test_04_post_warehouse(self):
        data = {
            "id": 99999,
            "code": "AAAAAA",
            "name": "AAAAA cargo hub",
            "address": "Wijnhaven 107",
            "zip": "4002 AS",
            "city": "Rotterdam",
            "province": "Zuid_Holland",
            "country": "NL",
            "contact": {
                "name": "Fem Keijzer",
                "phone": "(078) 0013363",
                "email": "blamore@example.net"
            },
            "created_at": "",
            "updated_at": ""
        }

        # Stuur de request
        response = self.client.post(url=(self.url + "/warehouses"), headers=self.headers, json=data)

        # Check de status code
        self.assertEqual(response.status_code, 201)


    
    # Overschrijft een warehouse op basis van de opgegeven warehouse-id
    def test_05_put_warehouse_id(self):
        data = {
            "id": 2,
            "code": "Overschrijf",
            "name": "Overschrijf cargo hub",
            "address": "Wijnhaven 107",
            "zip": "4002 AS",
            "city": "Rotterdam",
            "province": "Zuid_Holland",
            "country": "NL",
            "contact": {
                "name": "Fem Keijzer",
                "phone": "(078) 0013363",
                "email": "blamore@example.net"
            },
            "created_at": "",
            "updated_at": ""
        }

        # Stuur de request
        response = self.client.put(url=(self.url + "/warehouses/2"), headers=self.headers, json=data)

        # Check de status code
        self.assertEqual(response.status_code, 200)



        # deze delete een warehouse op basis van een id
    def test_06_delete_warehouse_id(self):
        # Stuur de request
        response = self.client.delete(url=(self.url + "/warehouses/99999"), headers=self.headers)

        # Check de status code
        self.assertEqual(response.status_code, 200)

    
    # Unhappy
    def test_07_post_existing_warehouse(self):
        data = {
            "id": 4,
            "code": "Already exist!!!",
            "name": "Bosch en Duin storage location",
            "address": "Fabianweg 71",
            "zip": "5701 IA",
            "city": "Bosch en Duin",
            "province": "Flevoland",
            "country": "NL",
            "contact": {
                "name": "Oscar Hemma van Allemani\u00eb-Hoes",
                "phone": "058 2995479",
                "email": "suze00@example.org"
            },
            "created_at": "",
            "updated_at": ""
        }
        
        response = self.client.post(url=(self.url + "/warehouses"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 400)

    
    
    # Unhappy
    def test_08_post_invalid_warehouse(self):
        data = {
            "id": 5,
            "wrong_property": "wrong"
        }
        
        response = self.client.post(url=(self.url + "/warehouses"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 400)

 
# to run the file: python -m unittest test_warehouses.py
# # git checkout . -f