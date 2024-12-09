import httpx
import unittest

def check_inventory(inventory):

    if len(inventory) != 12:
        return False

    if inventory.get("id") == None:
        return False
    if inventory.get("item_id") == None:
        return False
    if inventory.get("description") == None:
        return False
    if inventory.get("item_reference") == None:
        return False
    if inventory.get("locations") == None:
        return False
    if inventory.get("total_on_hand") == None:
        return False
    if inventory.get("total_expected") == None:
        return False
    if inventory.get("total_ordered") == None:
        return False
    if inventory.get("total_allocated") == None:
        return False
    if inventory.get("total_available") == None:
        return False
    if inventory.get("created_at") == None:
        return False
    if inventory.get("updated_at") == None:
        return False

    return True




class TestClass(unittest.TestCase):
    def setUp(self):
        self.client = httpx.Client()
        self.url = "http://localhost:3000/api/v1"
        self.headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        # Ensure the test data is available
        self.client.post(url=(self.url + "/inventories"), headers=self.headers, json={
            "id": 1,
            "item_id": "P000001",
            "description": "Test Inventory",
            "item_reference": "ref001",
            "locations": [1, 2, 3],
            "total_on_hand": 100,
            "total_expected": 50,
            "total_ordered": 30,
            "total_allocated": 20,
            "total_available": 80,
            "created_at": "",
            "updated_at": ""
        })

    def tearDown(self):
        self.client.delete(url=(self.url + "/inventories/1"), headers=self.headers)
        self.client.close()


    def test_01_get_Inventories(self):
        
        response = self.client.get(url=(self.url + "/inventories"), headers=self.headers)
        
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), list)
        
        if (len(response.json()) > 0):
            # Check dat de response een dictionary is (representatief voor een enkel inventory object)
            self.assertEqual(type(response.json()[0]), dict)
            
            self.assertTrue(
                all(
                    check_inventory(inventory)
                    for inventory in response.json()
                )
            )


    def test_02_get_inventories_id(self):

        response = self.client.get(url=(self.url + "/inventories/1"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        # Check dat de response een dictionary is (representatief voor een enkel inventory object)
        self.assertEqual(type(response.json()), dict)
        # Check dat het inventory object de juiste properties heeft
        self.assertTrue(check_inventory(response.json()))


    # deze voegt een nieuwe inventory object
    def test_03_post_inventories(self):    
        data = {
            "id": 2,
            "item_id": "P000002",
            "description": "New Inventory",
            "item_reference": "ref002",
            "locations": [
                1,
                2,
                3
            ],
            "total_on_hand": 200,
            "total_expected": 100,
            "total_ordered": 60,
            "total_allocated": 40,
            "total_available": 160,
            "created_at": "",
            "updated_at": ""
        }

        response = self.client.post(url=(self.url + "/inventories"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 201)


    
    # Overschrijft een inventory op basis van de opgegeven inventory-id
    def test_04_put_inventory_id(self):
        data = {
            "id": 2,
            "item_id": "P000002",
            "description": "Focused transitional alliance",
            "item_reference": "nyg48736S",
            "locations": [
                1,
                2,
                3,
                4,
                5
            ],
            "total_on_hand": 9999,
            "total_expected": 9999,
            "total_ordered": 9999,
            "total_allocated": 999,
            "total_available": 9999,
            "created_at": "",
            "updated_at": ""
        }

        response = self.client.put(url=(self.url + "/inventories/2"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 200)


    def test_06_delete_inventory_id(self):
        response = self.client.delete(url=(self.url + "/inventories/1"), headers=self.headers)
        self.assertEqual(response.status_code, 200)


    # Unhappy
    def test_07_post_existing_inventory(self):
        data ={
            "id": 4,
            "item_id": "P000004",
            "description": "Pre-emptive asynchronous throughput",
            "item_reference": "zdN19039A",
            "locations": [
                1,
                2,
                3,
                4,
                5
            ],
            "total_on_hand": 124,
            "total_expected": 0,
            "total_ordered": 106,
            "total_allocated": 0,
            "total_available": 18,
            "created_at": "",
            "updated_at": ""
        }

        response = self.client.post(url=(self.url + "/inventories"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 400)

    
    
    # Unhappy
    def test_08_post_invalid_inventory(self):
        data = {
            "id": 6,
            "wrong_property": "wrong"
        }
        
        response = self.client.post(url=(self.url + "/inventories"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 400)



# to run the file: python -m unittest test_inventories.py   ---> Tests/Integration
# git checkout . -f  ---> test_data
