import httpx
import unittest

def checkItemLine(item_line):
    json_entry = [
        "id", "name", "description", "created_at", "updated_at"
    ]
    for option in json_entry:
        if item_line.get(option) is None:
            return False

    if len(item_line) != 5:
        return False
    return True


def checkItem(item):
    json_entry = [
        "uid", "code", "description", "short_description", "upc_code",
        "model_number", "commodity_code", "item_line", "item_group",
        "item_type", "unit_purchase_quantity", "unit_order_quantity",
        "pack_order_quantity", "supplier_id", "supplier_code", "supplier_part_number",
        "created_at", "updated_at"
    ]
    for option in json_entry:
        if item.get(option) is None:
            return False

    if len(item) != 18:
        return False
    return True

class TestItemLines(unittest.TestCase):
    def setUp(self):
        self.headers = httpx.Headers({'API_KEY': 'a1b2c3d4e5'})
        self.client = httpx
        self.url = "http://localhost:3000/api/v1"

    def test_01_get_item_lines(self):
        response = self.client.get(url=(self.url + "/item_lines"), headers=self.headers)
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), list)

        if len(response.json()) > 0:
            self.assertEqual(type(response.json()[0]), dict)
            self.assertTrue(checkItemLine(response.json()[0]))

    def test_02_get_item_line_by_id(self):
        response = self.client.get(url=(self.url + "/item_lines/2"), headers=self.headers)
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), dict)

        if len(response.json()) > 0:
            self.assertEqual(type(response.json()), dict)
            self.assertTrue(checkItemLine(response.json()))

    def test_03_get_items_by_item_line_id(self):
        response = self.client.get(url=(self.url + "/item_lines/1/items"), headers=self.headers)
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), list)

        if len(response.json()) > 0:
            self.assertEqual(type(response.json()[0]), dict)

        self.assertTrue(
            all(checkItem(item) 
            for item in response.json())
        )

    def test_04_put_item_line(self):
        data = {
            "id": 2,
            "name": "Updated Item Line",
            "description": "",
            "created_at": "",
            "updated_at": ""
        }

        response = self.client.put(url=(self.url + "/item_lines/2"), headers=self.headers, json=data)
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 200)

    def test_05_delete_item_line(self):
        response = self.client.delete(url=(self.url + "/item_lines/7"), headers=self.headers)
        # print(response.text)  # Debugging information for delete
        self.assertEqual(response.status_code, 200)