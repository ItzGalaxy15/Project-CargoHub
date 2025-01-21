import httpx
import unittest

def checkItemLine(item_line):
    json_entry = [
        "id", "name", "description", "created_at", "updated_at"
    ]
    for option in json_entry:
        if item_line.get(option) is None:
            return False

    if len(item_line) != 6:
        return False
    return True


def checkItemLinesByID(item_line):
    item_id = item_line.get("id")
    if item_id is None or item_id <= 1:
        return False
    else:
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

    if len(item) != 19:
        return False
    return True


class TestClass(unittest.TestCase):
    def setUp(self):
        self.item_lines = httpx.Client(headers={'API_KEY': 'a1b2c3d4e5'})
        self.url = "http://localhost:3000/api/v2"

    def tearDown(self):
        self.item_lines.close()

    def test_01_get_item_lines(self):
        response = self.item_lines.get(f"{self.url}/item_lines")
        
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), list)

        if len(response.json()) > 0:
            self.assertEqual(type(response.json()[0]), dict)
            self.assertTrue(checkItemLine(response.json()[0]))

    def test_02_get_item_line_by_id(self):
        response = self.item_lines.get(f"{self.url}/item_lines/2")
        
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), dict)

        if len(response.json()) > 0:
            self.assertEqual(type(response.json()), dict)
            self.assertTrue(checkItemLine(response.json()))

    def test_03_get_items_by_item_line_id(self):
        response = self.item_lines.get(f"{self.url}/item_lines/14/items")

        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), list)

        if len(response.json()) > 0:
            self.assertEqual(type(response.json()[0]), dict)
            self.assertTrue(checkItemLinesByID(response.json()[0]))

    def test_04_put_item_line(self):
        data = {
            "id": 1,
            "name": "Updated Item Line",
            "description": "",
            "created_at": "",
            "updated_at": ""
        }

        response = self.item_lines.put(f"{self.url}/item_lines/1", json=data)
        
        # print(response.text)
        self.assertEqual(response.status_code, 200)

    def test_05_delete_item_line(self):
        response = self.item_lines.post(f"{self.url}/item_lines", 
            json={
                "id": 15,
                "name": "bob",
                "description": "no",
                "created_at": "2024-10-01 02:22:53",
                "updated_at": "2024-10-02 20:22:35"
                }
        )
        response = self.item_lines.delete(f"{self.url}/item_lines/15")
        
        self.assertEqual(response.status_code, 200)
        print(response.text)  # Debugging information for delete