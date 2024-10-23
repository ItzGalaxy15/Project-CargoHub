import httpx
import unittest

def checkItem(item):
    required_properties = [
        "uid", "code", "description", "short_description", "upc_code", 
        "model_number", "commodity_code", "item_line", "item_group", 
        "item_type", "unit_purchase_quantity", "unit_order_quantity", 
        "pack_order_quantity", "supplier_id", "supplier_code", 
        "supplier_part_number", "created_at", "updated_at"
    ]
    for prop in required_properties:
        if item.get(prop) is None:
            return False
    return True

def checkInventoryItem(item):
    required_properties = [
        "id", "item_id", "description", "item_reference", "locations", 
        "total_on_hand", "total_expected", "total_ordered", 
        "total_allocated", "total_available", "created_at", "updated_at"
    ]
    for prop in required_properties:
        if item.get(prop) is None:
            print(f"Missing property: {prop}")
            return False
    return True

class TestItemClass(unittest.TestCase):
    def setUp(self):
        self.client = httpx.Client()
        self.url = "http://localhost:3000/api/v1"
        self.headers = httpx.Headers({'API_KEY': 'a1b2c3d4e5'})

    def tearDown(self):
        self.client.close()

    def test_01_get_items(self):
        response = self.client.get(url=(self.url + "/items"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), list)
        if len(response.json()) > 0:
            self.assertEqual(type(response.json()[0]), dict)
            self.assertTrue(all(checkItem(item) for item in response.json()))

    def test_02_get_item_by_id(self):
        response = self.client.get(url=(self.url + "/items/P000001"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), dict)
        self.assertTrue(checkItem(response.json()))

    def test_03_get_item_totals_by_uid(self):
        response = self.client.get(url=(self.url + "/items/P000001/inventory/totals"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), dict)

    def test_04_get_inventory_by_uid(self):
        response = self.client.get(url=(self.url + "/items/P000002/inventory"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), dict)
        self.assertTrue(checkInventoryItem(response.json()))

    def test_05_post_item(self):
        data = {
            "uid": "P011722",
            "code": "newCode",
            "description": "New item description",
            "short_description": "New short description",
            "upc_code": "123456789012",
            "model_number": "model123",
            "commodity_code": "comm123",
            "item_line": 1,
            "item_group": 1,
            "item_type": 1,
            "unit_purchase_quantity": 10,
            "unit_order_quantity": 5,
            "pack_order_quantity": 2,
            "supplier_id": 1,
            "supplier_code": "SUP123",
            "supplier_part_number": "PART123",
            "created_at": "2023-01-01T00:00:00Z",
            "updated_at": "2023-01-01T00:00:00Z"
        }
        response = self.client.post(url=(self.url + "/items"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 200)

    def test_06_put_item_by_id(self):
        data = {
            "uid": "P000001",
            "code": "updatedCode",
            "description": "Updated item description",
            "short_description": "Updated short description",
            "upc_code": "123456789012",
            "model_number": "model123",
            "commodity_code": "comm123",
            "item_line": 1,
            "item_group": 1,
            "item_type": 1,
            "unit_purchase_quantity": 10,
            "unit_order_quantity": 5,
            "pack_order_quantity": 2,
            "supplier_id": 1,
            "supplier_code": "SUP123",
            "supplier_part_number": "PART123",
            "created_at": "2023-01-01T00:00:00Z",
            "updated_at": "2023-01-01T00:00:00Z"
        }
        response = self.client.put(url=(self.url + "/items/P000001"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 200)

    def test_07_delete_item_by_id(self):
        response = self.client.delete(url=(self.url + "/items/P011722"), headers=self.headers)
        self.assertEqual(response.status_code, 200)

    def test_08_post_existing_item(self):
        data = {
            "uid": "P000005",
            "code": None,
            "description": None,
            "short_description": None,
            "upc_code": None,
            "model_number": None,
            "commodity_code": None,
            "item_line": None,
            "item_group": None,
            "item_type": None,
            "unit_purchase_quantity": None,
            "unit_order_quantity": None,
            "pack_order_quantity": None,
            "supplier_id": None,
            "supplier_code": None,
            "supplier_part_number": None,
            "created_at": None,
            "updated_at": None
        }
        response = self.client.post(url=(self.url + "/items"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 400)
        
        response = self.client.get(url=(self.url + "/items/P000005"), headers=self.headers)
        self.assertNotEqual(response.json()["code"], "ABBC")
        
        
    def test_09_post_invalid_item(self):
        data = {
            "uid": "P000002",
            "wrong_property": "wrong"
        }
        response = self.client.post(url=(self.url + "/items"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 400)

if __name__ == "__main__":
    unittest.main()