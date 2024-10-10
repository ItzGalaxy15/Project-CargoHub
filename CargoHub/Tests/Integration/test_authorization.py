import httpx
import unittest

# TESTS VOOR LIMITED KEY WERKEN NIET, OMDAT DE AUTHORIZATION METHOD NIET WERKT
# Het doet access[path][method], maar het moet access[path[0]][method] zijn

class TestClass(unittest.TestCase):
    
    def setUp(self):
        self.client = httpx
        self.url = "http://localhost:3000/api/v1/"
        
    
    def test_get_full_key(self):
        # Set API_KEY for Dashboard 1
        headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        response = self.client.get(url=self.url + "warehouses", headers=headers)
        
        # Check voor 200 (gelukt)
        self.assertEqual(response.status_code, 200)
        
    
    def test_get_limited_key(self):
        # Set API_KEY for Dashboard 2
        headers = httpx.Headers({ 'API_KEY': 'f6g7h8i9j0' })
        response = self.client.get(url=self.url + "warehouses", headers=headers)
        
        # Check voor 403 (niet authorized)
        self.assertEqual(response.status_code, 200)
        
    
    def test_get_no_key(self):
        response = self.client.get(url=self.url)
        
        # Check voor 401 (geen api key)
        self.assertEqual(response.status_code, 401)
        

    def test_post_full_key(self):
        # Set API_KEY for Dashboard 1
        headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        response = self.client.post(url=self.url + "warehouses", headers=headers, json=dict())
        
        # Check voor 201 (gelukt)
        self.assertEqual(response.status_code, 201)
        
    
    def test_post_limited_key(self):
        # Set API_KEY for Dashboard 2
        headers = httpx.Headers({ 'API_KEY': 'f6g7h8i9j0' })
        response = self.client.post(url=self.url + "warehouses", headers=headers, json=dict())
        
        # Check voor 403 (niet authorized)
        self.assertEqual(response.status_code, 403)
        
    
    def test_post_no_key(self):
        response = self.client.post(url=self.url, json=dict())
        
        # Check voor 401 (geen api key)
        self.assertEqual(response.status_code, 401)
    
    
    def test_put_full_key(self):
        # Set API_KEY for Dashboard 1
        headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        response = self.client.put(url=self.url + "warehouses/1", headers=headers, json={"id": 1})
        
        # Check voor 200 (gelukt)
        self.assertEqual(response.status_code, 200)
        
    
    def test_put_limited_key(self):
        # Set API_KEY for Dashboard 2
        headers = httpx.Headers({ 'API_KEY': 'f6g7h8i9j0' })
        response = self.client.put(url=self.url + "warehouses/", headers=headers)
        
        # Check voor 403 (niet authorized)
        self.assertEqual(response.status_code, 403)
        
    
    def test_put_no_key(self):
        response = self.client.put(url=self.url)
        
        # Check voor 401 (geen api key)
        self.assertEqual(response.status_code, 401)
    
    
    def test_delete_full_key(self):
        # Set API_KEY for Dashboard 1
        headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        response = self.client.delete(url=self.url + "warehouses/2", headers=headers)
        
        # Check voor 200 (gelukt)
        self.assertEqual(response.status_code, 200)
        
    
    def test_delete_limited_key(self):
        # Set API_KEY for Dashboard 2
        headers = httpx.Headers({ 'API_KEY': 'f6g7h8i9j0' })
        response = self.client.delete(url=self.url + "warehouses", headers=headers)
        
        # Check voor 403 (niet authorized)
        self.assertEqual(response.status_code, 403)
        
    
    def test_delete_no_key(self):
        response = self.client.delete(url=self.url)
        
        # Check voor 401 (geen api key)
        self.assertEqual(response.status_code, 401)
