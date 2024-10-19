import unittest
import httpx

item_properties = [
    "uid", "name", "item_line", "item_group", "item_type", "supplier_id", "created_at", "updated_at"
]

def check_item(item):
    # Check if item has the right amount of properties
    if len(item) != len(item_properties):
        return False

    # Check if item has the right properties
    for property in item_properties:
        if property not in item:
            return False

    return True

class TestItems(unittest.TestCase):
    def setUp(self):
        self.client = httpx
        self.base_url = "http://localhost:3000/api/v1"
        self.headers = httpx.Headers({'API_KEY': 'a1b2c3d4e5'})

    def test_get_items(self):
        response = self.client.get(url=(self.base_url + "/items"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), list)
        if len(response.json()) > 0:
            self.assertTrue(check_item(response.json()[0]))

    def test_get_item(self):
        item_id = 1  # Example item ID
        response = self.client.get(url=(self.base_url + f"/items/{item_id}"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), dict)
        self.assertTrue(check_item(response.json()))
        self.assertEqual(response.json()["uid"], item_id)

    def test_post_item(self):
        new_item = {
            "uid": 6,
            "name": "NewItem",
            "item_line": 1,
            "item_group": 1,
            "item_type": 1,
            "supplier_id": 1,
            "created_at": None,
            "updated_at": None
        }
        response = self.client.post(url=(self.base_url + "/items"), headers=self.headers, json=new_item)
        self.assertEqual(response.status_code, 201)
        self.assertTrue(check_item(response.json()))

        response = self.client.get(url=(self.base_url + "/items/6"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(response.json()["uid"], 6)

    def test_put_item(self):
        item_id = 1  # Example item ID
        updated_item = {
            "uid": item_id,
            "name": "UpdatedItem",
            "item_line": 1,
            "item_group": 1,
            "item_type": 1,
            "supplier_id": 1,
            "created_at": None,
            "updated_at": None
        }
        response = self.client.put(url=(self.base_url + f"/items/{item_id}"), headers=self.headers, json=updated_item)
        self.assertEqual(response.status_code, 200)
        self.assertTrue(check_item(response.json()))

        response = self.client.get(url=(self.base_url + f"/items/{item_id}"), headers=self.headers)
        self.assertEqual(response.json()["name"], "UpdatedItem")

    def test_delete_item(self):
        item_id = 5  # Example item ID
        response = self.client.delete(url=(self.base_url + f"/items/{item_id}"), headers=self.headers)
        self.assertEqual(response.status_code, 204)

        response = self.client.get(url=(self.base_url + f"/items/{item_id}"), headers=self.headers)
        self.assertEqual(response.status_code, 404)

#######################################################################################################################

    #unhappy tests, existing item id & invalid structure

    def test_post_item_existing_id(self):
        existing_item = {
            "uid": 1,  # Assuming 1 already exists
            "name": "ExistingItem",
            "item_line": 1,
            "item_group": 1,
            "item_type": 1,
            "supplier_id": 1,
            "created_at": None,
            "updated_at": None
        }
        response = self.client.post(url=(self.base_url + "/items"), headers=self.headers, json=existing_item)
        self.assertEqual(response.status_code, 400)  # Assuming 400 Bad Request for duplicate ID

    def test_post_item_invalid_structure(self):
        invalid_item = {
            "uid": 7,
            "name": "InvalidItem"
            # Missing other required fields
        }
        response = self.client.post(url=(self.base_url + "/items"), headers=self.headers, json=invalid_item)
        self.assertEqual(response.status_code, 400)  # Assuming 400 Bad Request for invalid structure


if __name__ == '__main__':
    unittest.main()