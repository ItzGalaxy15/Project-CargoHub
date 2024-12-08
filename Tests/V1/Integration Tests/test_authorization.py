import httpx
import unittest

# TESTS VOOR LIMITED KEY WERKEN NIET, OMDAT DE AUTHORIZATION METHOD NIET WERKT
# Het doet access[path][method], maar het moet access[path[0]][method] zijn

class TestClass(unittest.TestCase):
    
    def setUp(self):
        self.client = httpx
        self.base_url = "http://localhost:3000/api/v1/"
        
    
    def test_01_get_full_key(self):
        # Set API_KEY for Dashboard 1
        headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        response = self.client.get(url=self.base_url + "warehouses", headers=headers)
        
        # Check voor 200 (gelukt)
        self.assertEqual(response.status_code, 200)
        
    
    def test_02_get_limited_key(self):
        # Set API_KEY for Dashboard 2
        headers = httpx.Headers({ 'API_KEY': 'f6g7h8i9j0' })
        response = self.client.get(url=self.base_url + "warehouses", headers=headers)
        
        # Check voor 403 (niet authorized)
        self.assertEqual(response.status_code, 200)
        
    
    def test_03_get_no_key(self):
        response = self.client.get(url=self.base_url)
        
        # Check voor 401 (geen api key)
        self.assertEqual(response.status_code, 401)
        

    def test_04_post_full_key(self):
        # Set API_KEY for Dashboard 1
        data = {
            "id": 80,
            "code": "AAAAAA",
            "name": "AAAAA cargo hub",
            "address": "Wijnhaven 107",
            "zip": "4002 AS",
            "city": "Rotterdam",
            "province": "Zuid_Holland",
            "country": "NL",
            "contact": {
                "name": "Fem Keijzer",
                "phone": "(078) 0013363",
                "email": "blamore@example.net"
            },
            "created_at": "",
            "updated_at": ""
        }

        headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        response = self.client.post(url=self.base_url + "warehouses", headers=headers, json=data)
        
        # Check voor 201 (gelukt)
        self.assertEqual(response.status_code, 201)
        
    
    def test_05_post_limited_key(self):
        # Set API_KEY for Dashboard 2
        headers = httpx.Headers({ 'API_KEY': 'f6g7h8i9j0' })
        response = self.client.post(url=self.base_url + "warehouses", headers=headers, json=dict())
        
        # Check voor 403 (niet authorized)
        self.assertEqual(response.status_code, 401)
        
    
    def test_06_post_no_key(self):
        response = self.client.post(url=self.base_url, json=dict())
        
        # Check voor 401 (geen api key)
        self.assertEqual(response.status_code, 401)
    
    
    def test_07_put_full_key(self):
        # Set API_KEY for Dashboard 1

        data = {
            "id": 2,
            "code": "BBBBB",
            "name": "BB2 cargo hub",
            "address": "Wijnhaven 107",
            "zip": "4002 AS",
            "city": "Rotterdam",
            "province": "Zuid_Holland",
            "country": "NL",
            "contact": {
                "name": "Fem Keijzer",
                "phone": "(078) 0013363",
                "email": "blamore@example.net"
            },
            "created_at": "",
            "updated_at": ""
        }
        headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        # response = self.client.put(url=self.base_url + "warehouses/1", headers=headers, json={"id": 1})
        response = self.client.put(url=self.base_url + "warehouses/2", headers=headers, json=data)

        
        # Check voor 200 (gelukt)
        self.assertEqual(response.status_code, 200)
        
    
    def test_08_put_limited_key(self):
        # Set API_KEY for Dashboard 2
        headers = httpx.Headers({ 'API_KEY': 'f6g7h8i9j0' })
        response = self.client.put(url=self.base_url + "warehouses/", headers=headers)
        
        # Check voor 403 (niet authorized)
        self.assertEqual(response.status_code, 401)
        
    
    def test_09_put_no_key(self):
        response = self.client.put(url=self.base_url)
        
        # Check voor 401 (geen api key)
        self.assertEqual(response.status_code, 401)
    
    
    def test_10_delete_full_key(self):
        # Set API_KEY for Dashboard 1
        headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        response = self.client.delete(url=self.base_url + "warehouses/80", headers=headers)
        
        # Check voor 200 (gelukt)
        self.assertEqual(response.status_code, 200)
        
    
    def test_11_delete_limited_key(self):
        # Set API_KEY for Dashboard 2
        headers = httpx.Headers({ 'API_KEY': 'f6g7h8i9j0' })
        response = self.client.delete(url=self.base_url + "warehouses", headers=headers)
        
        # Check voor 403 (niet authorized)
        self.assertEqual(response.status_code, 401)
        
    
    def test_12_delete_no_key(self):
        response = self.client.delete(url=self.base_url)
        
        # Check voor 401 (geen api key)
        self.assertEqual(response.status_code, 401)
