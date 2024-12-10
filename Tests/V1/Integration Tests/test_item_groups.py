import httpx
import unittest

# def check_item_groups(item_groups):

#     if len(item_groups) != 5:
#         return False

#     if item_groups.get("id") == None:
#         return False
#     if item_groups.get("name") == None:
#         return False
#     if item_groups.get("description") == None:
#         return False
#     if item_groups.get("created_at") == None:
#         return False
#     if item_groups.get("updated_at") == None:
#         return False

#     return True



# def check_items(item):

#     if len(item) != 18:
#         return False

#     if item.get("uid") == None:
#         return False
#     if item.get("code") == None:
#         return False
#     if item.get("description") == None:
#         return False
#     if item.get("short_description") == None:
#         return False
#     if item.get("upc_code") == None:
#         return False
#     if item.get("model_number") == None:
#         return False
#     if item.get("commodity_code") == None:
#         return False
#     if item.get("item_line") == None:
#         return False
#     if item.get("item_group") == None:
#         return False
#     if item.get("item_type") == None:
#         return False
#     if item.get("unit_purchase_quantity") == None:
#         return False
#     if item.get("unit_order_quantity") == None:
#         return False
#     if item.get("pack_order_quantity") == None:
#         return False
#     if item.get("supplier_id") == None:
#         return False
#     if item.get("supplier_code") == None:
#         return False
#     if item.get("supplier_part_number") == None:
#         return False
#     if item.get("created_at") == None:
#         return False
#     if item.get("updated_at") == None:
#         return False

#     return True


class TestClass(unittest.TestCase):
    def setUp(self):
        self.client = httpx
        self.url = "http://localhost:3000/api/v1"
        self.headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })


    def test_01_get_item_groups(self):
        
        response = self.client.get(url=(self.url + "/item_groups"), headers=self.headers)
        
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een list is
        self.assertEqual(type(response.json()), list)
        
        # Als de list iets bevat (want een list van 0 objects is inprincipe "legaal")
        # if (len(response.json()) > 0):
        #     # Check of de object in de list ook echt een "object" (eigenlijk overal een dictionary) is,
        #     # dus niet dat het een list van ints, strings etc. zijn
        #     self.assertEqual(type(response.json()[0]), dict)

        #     # Check dat elk item-groups de juiste eigenschappen heeft
        #     self.assertTrue(
        #         all(
        #             check_item_groups(item_groups)
        #             for item_groups in response.json()
        #         )
        #     )


    def test_02_get_item_groups_id(self):
        response = self.client.get(url=(self.url + "/item_groups/1"), headers=self.headers)
        
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een dictionary is (representatief voor een enkel item_groups object)
        self.assertEqual(type(response.json()), dict)
        
        # Check dat het item_groups object de juiste properties heeft
        # self.assertTrue(check_item_groups(response.json()))


    def test_03_get_item_groups_id_items(self):
        # Stuur de request
        response = self.client.get(url=(self.url + "/item_groups/1/items"), headers=self.headers)
        
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een list is (representatief voor een lijst met items)
        self.assertEqual(type(response.json()), list)
        
        # Als de lijst iets bevat, controleer dan het eerste object in de lijst
        # if len(response.json()) > 0:
        #     # Check dat elk object in de lijst een dictionary is
        #     self.assertEqual(type(response.json()[0]), dict)
            
        #     # Check dat elk item-object de juiste eigenschappen heeft
        #     self.assertTrue(
        #         all(
        #             check_items(item)
        #             for item in response.json()
        #         )
        #     )

    
    # Overschrijft een item_groups op basis van de opgegeven item_groups-id
    def test_04_put_item_groups_id(self):
        data = {
            "id": 2,
            "name": "Overschrijf",
            "description": "",
            "created_at": "",
            "updated_at": ""
        }
        
        response = self.client.put(url=(self.url + "/item_groups/2"), headers=self.headers, json=data)
        self.assertEqual(response.status_code, 200)

        # deze delete een item_groups op basis van een id
    def test_05_delete_item_groups_id(self):
        response = self.client.post(url=(self.url + "/item_groups"), headers=self.headers,
            json = {
                "id": 9,
                "name": "Overschrijf",
                "description": "",
                "created_at": "",
                "updated_at": ""
            }
        )
        # Stuur de request
        response = self.client.delete(url=(self.url + "/item_groups/9"), headers=self.headers)

        # Check de status code
        self.assertEqual(response.status_code, 200)



# to run the file: python -m unittest test_item_groups.py
# git checkout . -f

if __name__ == "__main__":
    unittest.main()