import httpx
import unittest



class TestItems(unittest.TestCase):
    def setUp(self):
        self.headers = httpx.Headers({'API_KEY': 'a1b2c3d4e5'})
        self.client = httpx
        self.url = "http://localhost:3000/api/v1"


    def test_01_get_items(self):
        response = self.client.get(url=(self.url + "/items"), headers=self.headers)
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), list)

        if len(response.json()) > 0:
            self.assertEqual(type(response.json()[0]), dict)
            self.assertTrue(checkItem(response.json()[0]))

    def test_02_get_item_by_id(self):
        response = self.client.get(url=(self.url + "/items/P000005"), headers=self.headers)
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), dict)

        if len(response.json()) > 0:
            self.assertEqual(type(response.json()), dict)
            self.assertTrue(checkItem(response.json()))

    def test_03_get_item_totals_by_uid(self):
        response = self.client.get(url=(self.url + "/items/1/inventory/totals"), headers=self.headers)
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), dict)

    def test_04_get_inventory_by_uid(self):
        response = self.client.get(url=(self.url + "/items/3/inventory"), headers=self.headers)
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), dict)

    def test_05_post_item(self):
        data = {
            "uid": "P011126",
            "code": "newCode",
            "description": "New Item Description",
            "short_description": "New",
            "upc_code": "1234567890123",
            "model_number": "newModel",
            "commodity_code": "newCommodity",
            "item_line": 1,
            "item_group": 1,
            "item_type": 1,
            "unit_purchase_quantity": 10,
            "unit_order_quantity": 5,
            "pack_order_quantity": 2,
            "supplier_id": 1,
            "supplier_code": "SUP001",
            "supplier_part_number": "SPN001",
            "created_at": "2023-10-01 00:00:00",
            "updated_at": "2023-10-01 00:00:00"
        }

        response = self.client.post(url=(self.url + "/items"), headers=self.headers, json=data)
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 201)


    def test_06_put_item(self):
        data = {
            "uid": "P000003",
            "code": "updatedCode",
            "description": "Updated Item Description",
            "short_description": "Updated",
            "upc_code": "1234567890123",
            "model_number": "updatedModel",
            "commodity_code": "updatedCommodity",
            "item_line": 1,
            "item_group": 1,
            "item_type": 1,
            "unit_purchase_quantity": 10,
            "unit_order_quantity": 5,
            "pack_order_quantity": 2,
            "supplier_id": 1,
            "supplier_code": "SUP001",
            "supplier_part_number": "SPN001",
            "created_at": "2023-10-01 00:00:00",
            "updated_at": "2023-10-01 00:00:00"
        }

        response = self.client.put(url=(self.url + "/items/P000003"), headers=self.headers, json=data)
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 200)

    def test_07_delete_item(self):
        response = self.client.delete(url=(self.url + "/items/P011126"), headers=self.headers)
        # print(response.text)  # Debugging information for delete
        self.assertEqual(response.status_code, 200)


######################################################################################################################################
#unhappy test item -> existing id, invalid structure

    def test_08_unhappy_post_existing_id(self):
        existing_item = {
            "uid": "P000003",
            "code": "sjQ23408K",
            "description": "Face-to-face clear-thinking complexity",
            "short_description": "must",
            "upc_code": "6523540947122",
            "model_number": "63-OFFTq0T",
            "commodity_code": "oTo304",
            "item_line": 11,
            "item_group": 73,
            "item_type": 14,
            "unit_purchase_quantity": 47,
            "unit_order_quantity": 13,
            "pack_order_quantity": 11,
            "supplier_id": 34,
            "supplier_code": "SUP423",
            "supplier_part_number": "E-86805-uTM",
            "created_at": "2015-02-19 16:08:24",
            "updated_at": "2015-09-26 06:37:56"
        }

        # First, create the item to ensure it exists
        response = self.client.post(url=(self.url + "/items"), headers=self.headers, json=existing_item)
        # print("Create Response:", response_create.status_code, response_create.text)  # Debugging information
        self.assertEqual(response.status_code, 400)

    def test_09_unhappy_post_invalid_structure(self):
        data = {
            "uid": "P000008",
            "code": 123,  # Invalid type
            "description": "Invalid Item Description",
            "short_description": "Invalid",
            "upc_code": "1234567890123",
            "model_number": "invalidModel",
            "commodity_code": "invalidCommodity",
            "item_line": 1,
            "item_group": 1,
            "item_type": 1,
            "unit_purchase_quantity": 10,
            "unit_order_quantity": 5,
            "pack_order_quantity": 2,
            "supplier_id": 1,
            "supplier_code": "SUP001",
            "supplier_part_number": "SPN001",
            "created_at": "2023-10-01 00:00:00",
            "updated_at": "2023-10-01 00:00:00"
        }

        response = self.client.post(url=(self.url + "/items"), headers=self.headers, json=data)
        # print(response.text)  # Debugging information
        self.assertEqual(response.status_code, 400)
        
######################################################################################################################################


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

if __name__ == '__main__':
    unittest.main()