import httpx
import unittest

def checkItemTypes(item_type):
    json_entry = [
    "id", "name", "description","created_at", "updated_at"
    ]     
    for option in json_entry:
        if item_type.get(option) is None:
            return False
        
    if len(item_type) != 5:
        return False
    return True

def checkItemTypesId(item_type):
    if item_type.get("id") >= 0:
        return True
    else:
        return False

def checkItemTypesIdItems(item_type):
    json_entry = [
        "uid", "code", "description", "short_description", 
        "upc_code", "model_number", "commodity_code", "item_line", 
        "item_group", "item_type", "unit_purchase_quantity", "unit_order_quantity", 
        "pack_order_quantity", "supplier_id", "supplier_code", 
        "supplier_part_number", "created_at", "updated_at"
    ]
    for option in json_entry:
        if item_type.get(option) is None:
            return False
    return True

class TestClass(unittest.TestCase):
    def setUp(self):
        self.headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        self.item_types = httpx
        self.url = "http://localhost:3000/api/v1"


    def test_01_get_item_types(self):
        response = self.item_types.get(url=(self.url + "/item_types"), headers=self.headers)

        self.assertEqual(response.status_code, 200)

        self.assertEqual(type(response.json()), list)

        if (len(response.json()) > 0):
            self.assertEqual(type(response.json()[0]), dict)
            self.assertTrue(checkItemTypes(response.json()[0]))

    
    def test_02_get_item_types_id(self):
        response_id = self.item_types.get(url=(self.url + "/item_types/2"), headers=self.headers)

        self.assertEqual(response_id.status_code, 200)

        self.assertEqual(type(response_id.json()), dict) 

        if (len(response_id.json()) > 0):
            self.assertEqual(type(response_id.json()), dict)
            self.assertTrue(checkItemTypes(response_id.json()))
            self.assertTrue(checkItemTypesId(response_id.json()))
    
    def test_03_get_item_types_id_items(self):
        response_items = self.item_types.get(url=(self.url + "/item_types/1/items"), headers=self.headers)

        self.assertEqual(response_items.status_code, 200)

        self.assertEqual(type(response_items.json()), list)       

        if (len(response_items.json()) > 0):
            self.assertEqual(type(response_items.json()), list)
            self.assertTrue(checkItemTypesIdItems(response_items.json()[0]))

    def test_04_put_item_types(self):
        data = {
                "id": 1,
                "name": "jeff",
                "description": "jeff",
                "created_at": "2024-10-01 02:22:53",
                "updated_at": "2024-10-02 20:22:35"
                }
        
        response = self.item_types.put(url=(self.url + "/item_types/1"), headers=self.headers, json=data)

        self.assertEqual(response.status_code, 200)

    def test_05_delete_item_types(self):
        response_create = self.item_types.post(url=(self.url + "/item_types"), headers=self.headers, 
            json={
                "id": 200,
                "name": "jeff",
                "description": "jeff",
                "created_at": "2024-10-01 02:22:53",
                "updated_at": "2024-10-02 20:22:35"
                }
        )
        response = self.item_types.delete(url=(self.url + "/item_types/200"), headers=self.headers)
        
        self.assertEqual(response.status_code, 200)