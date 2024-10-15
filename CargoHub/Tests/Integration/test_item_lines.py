import unittest
import httpx

item_line_properties = [
    "id", "name", "description", "created_at", "updated_at"
]

def check_item_line(item_line):
    # Check if item_line has the right amount of properties
    if len(item_line) != len(item_line_properties):
        return False

    # Check if item_line has the right properties
    for property in item_line_properties:
        if property not in item_line:
            return False

    return True

class TestItemLines(unittest.TestCase):
    def setUp(self):
        self.client = httpx
        self.base_url = "http://localhost:3000/api/v1"
        self.headers = httpx.Headers({'API_KEY': 'a1b2c3d4e5'})

    def test_get_item_lines(self):
        response = self.client.get(url=(self.base_url + "/item_lines"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), list)

        if len(response.json()) > 0:
            self.assertEqual(type(response.json()[0]), dict)
            self.assertTrue(check_item_line(response.json()[0]))

    def test_get_item_line(self):
        item_line_id = 1  # Example item line ID
        response = self.client.get(url=(self.base_url + f"/item_lines/{item_line_id}"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), dict)
        self.assertTrue(check_item_line(response.json()))
        self.assertEqual(response.json()["id"], item_line_id)

    def test_post_item_line(self):
        new_item_line = {
            "id": 6,
            "name": "NewItemLine",
            "description": "New item line description",
            "created_at": None,
            "updated_at": None
        }
        response = self.client.post(url=(self.base_url + "/item_lines"), headers=self.headers, json=new_item_line)
        self.assertEqual(response.status_code, 201)

        response = self.client.get(url=(self.base_url + "/item_lines/6"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(response.json()["id"], 6)

    def test_put_item_line(self):
        item_line_id = 2  # Example item line ID
        updated_item_line = {
            "id": item_line_id,
            "name": "UpdatedItemLine",
            "description": "Updated item line description",
            "created_at": None,
            "updated_at": None
        }
        response = self.client.put(url=(self.base_url + f"/item_lines/{item_line_id}"), headers=self.headers, json=updated_item_line)
        self.assertEqual(response.status_code, 200)

        response = self.client.get(url=(self.base_url + f"/item_lines/{item_line_id}"), headers=self.headers)
        self.assertEqual(response.json()["name"], "UpdatedItemLine")

    def test_delete_item_line(self):
        item_line_id = 5  # Example item line ID
        response = self.client.delete(url=(self.base_url + f"/item_lines/{item_line_id}"), headers=self.headers)
        self.assertEqual(response.status_code, 204)

        response = self.client.get(url=(self.base_url + f"/item_lines/{item_line_id}"), headers=self.headers)
        self.assertEqual(response.status_code, 404)

if __name__ == '__main__':
    unittest.main()