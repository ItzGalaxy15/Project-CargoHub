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
        "id": 99999,
        "item_id": None,
        "description": None,
        "item_reference": None,
        "locations": None,
        "total_on_hand": None,
        "total_expected": None,
        "total_ordered": None,
        "total_allocated": None,
        "total_available": None,
        "created_at": None,
        "updated_at": None
        }

        response = self.client.post(url=(self.url + "/inventories"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 201)


    
    # Overschrijft een inventory op basis van de opgegeven inventory-id
    def test_04_put_inventory_id(self):
        data = {
        "id": 99999,
        "item_id": "AAAAA",
        "description": None,
        "item_reference": None,
        "locations": None,
        "total_on_hand": None,
        "total_expected": None,
        "total_ordered": None,
        "total_allocated": None,
        "total_available": None,
        "created_at": None,
        "updated_at": None
        }

        response = self.client.put(url=(self.url + "/inventories/2"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 200)


    def test_06_delete_inventory_id(self):
        response = self.client.delete(url=(self.url + "/inventories/3"), headers=self.headers)
        self.assertEqual(response.status_code, 200)


# to run the file: python -m unittest test_inventories.py   ---> Tests/Integration
# git checkout . -f  ---> test_data
