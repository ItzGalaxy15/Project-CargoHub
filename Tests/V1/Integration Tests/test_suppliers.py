import httpx
import unittest


supplier_properties = [
    "id", "code", "name", "address", "address_extra",
    "city", "zip_code", "province", "country", "contact_name",
    "phonenumber", "reference", "created_at", "updated_at",
]
def checkSupplier(supplier):

    # Check if supplier has right amount of properties
    if len(supplier) != len(supplier_properties):
        return False

    # Check if supplier has the right properties
    for property in supplier_properties:
        if property not in supplier:
            return False

    return True


class TestClass(unittest.TestCase):
    
    def setUp(self):
        self.client = httpx
        self.base_url = "http://localhost:3000/api/v1"
        self.headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })


    def test_01_get_suppliers(self):
        # Stuur de request
        response = self.client.get(url=(self.base_url + "/suppliers"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een list is
        self.assertEqual(type(response.json()), list)
        
        # Als de list iets bevat (want een list van 0 objects is inprincipe "valid")
        if (len(response.json()) > 0):
            # Check of de object in de list ook echt een "object" (eigenlijk overal een dictionary) is,
            # dus niet dat het een list van ints, strings etc. zijn
            self.assertEqual(type(response.json()[0]), dict)
            
            # Check dat de object de juiste properties heeft
            self.assertTrue(checkSupplier(response.json()[0]))
    
    
    def test_02_get_supplier(self):
        # Stuur de request
        response = self.client.get(url=(self.base_url + "/suppliers/1"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een object is
        self.assertEqual(type(response.json()), dict)

        # Check dat de object de juiste properties heeft
        self.assertTrue(checkSupplier(response.json()))
        self.assertEqual(response.json()["id"], 1)
    
    
    def test_03_get_supplier_items(self):
        # Stuur de request
        response = self.client.get(url=(self.base_url + "/suppliers/1/items"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)
        
        # Check dat de response een list is
        self.assertEqual(type(response.json()), list)
        
        # Als de list iets bevat (want een list van 0 objects is inprincipe "valid")
        if (len(response.json()) > 0):
            # Check of de list gevuld is met items
            # ! kan later gedaan worden met de function in item tests
            pass
            
            # Check of de supplier_id van de item 1 is
            self.assertEqual(response.json()[0]["supplier_id"], 1)
    
    
    def test_04_post_supplier(self):
        # Supplier object
        data = {
        "id": 111,
        "code": "SUP0006",
        "name": "Martin PLC",
        "address": "243 Henry Station Suite 090",
        "address_extra": "Suite 011",
        "city": "Smithview",
        "zip_code": "48427",
        "province": "New York",
        "country": "Guadeloupe",
        "contact_name": "James Mills MD",
        "phonenumber": "001-763-501-5416x14812",
        "reference": "MP-SUP0006",
        "created_at": "2019-10-28 00:58:28",
        "updated_at": "2019-12-28 10:23:09"
        }
        
        # Stuur de request
        response = self.client.post(url=(self.base_url + "/suppliers"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 201)
    
    
    def test_05_put_supplier(self):
        # Supplier object
        data = {
        "id": 2,
        "code": "SUP0006",
        "name": "Martin PLC",
        "address": "243 Henry Station Suite 090",
        "address_extra": "Suite 011",
        "city": "Smithview",
        "zip_code": "48427",
        "province": "New York",
        "country": "Guadeloupe",
        "contact_name": "James Mills MD",
        "phonenumber": "001-763-501-5416x14812",
        "reference": "MP-SUP0006",
        "created_at": "2019-10-28 00:58:28",
        "updated_at": "2019-12-28 10:23:09"
        }

        
        # Stuur de request
        response = self.client.put(url=(self.base_url + "/suppliers/2"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)

    
    
    def test_06_delete_supplier(self):
        # Stuur de request
        response = self.client.delete(url=(self.base_url + "/suppliers/111"), headers=self.headers)
        
        # Check de status code
        self.assertEqual(response.status_code, 200)


    # Unhappy
    def test_07_post_existing_supplier(self):
        # Supplier object
        data = {
        "id": 2,
        "code": "SUP0006",
        "name": "Martin PLC",
        "address": "243 Henry Station Suite 090",
        "address_extra": "Suite 011",
        "city": "Smithview",
        "zip_code": "48427",
        "province": "New York",
        "country": "Guadeloupe",
        "contact_name": "James Mills MD",
        "phonenumber": "001-763-501-5416x14812",
        "reference": "MP-SUP0006",
        "created_at": "2019-10-28 00:58:28",
        "updated_at": "2019-12-28 10:23:09"
        }
        
        # Stuur de request
        response = self.client.post(url=(self.base_url + "/suppliers"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 400)
    
    
    # Unhappy
    def test_08_post_invalid_supplier(self):
        # Supplier object
        data = {
            "id": 11,
            "wrong_property": "wrong"
        }
        
        # Stuur de request
        response = self.client.post(url=(self.base_url + "/suppliers"), headers=self.headers, json=data)
        
        # Check de status code
        self.assertEqual(response.status_code, 400)

#  python -m unittest test_suppliers.py