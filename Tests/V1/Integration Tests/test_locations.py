import httpx
import unittest

def checkLocation(location):
    json_entry = [
    "id", "warehouse_id", "code", "name", "created_at", 
    "updated_at"
    ]     
    for option in json_entry:
        if location.get(option) is None:
            return False
        
    if len(location) != 6:
        return False
    return True

def checklocationId(location):
    if location.get("id") == 1:
        return True
    else:
        return False

class TestClass(unittest.TestCase):
    def setUp(self):
        self.headers = httpx.Headers({ 'API_KEY': 'a1b2c3d4e5' })
        self.location = httpx
        self.url = "http://localhost:3000/api/v1"


    def test_01_get_locations(self):
        response = self.location.get(url=(self.url + "/locations"), headers=self.headers)

        self.assertEqual(response.status_code, 200)

        self.assertEqual(type(response.json()), list)       

        if (len(response.json()) > 0):
            self.assertEqual(type(response.json()[0]), dict)
            self.assertTrue(checkLocation(response.json()[0]))


    def test_02_get_location_id(self):
        response_id = self.location.get(url=(self.url + "/locations/3"), headers=self.headers)

        self.assertEqual(response_id.status_code, 200)    

        self.assertEqual(type(response_id.json()), dict)        


        if (len(response_id.json()) > 0):
            self.assertEqual(type(response_id.json()), dict)
            self.assertTrue(checkLocation(response_id.json()))
            self.assertTrue(checklocationId(response_id.json()))
    
    def test_03_post_location(self):
        data = {
                "id": 600000,
                "warehouse_id": "60",
                "code": "B.3.0",
                "name": "Row: B, Rack: 3, Shelf: 0",
                "created_at": "1992-05-15 03:21:32",
                "updated_at": "1992-05-15 03:21:32",
                }
        
        response = self.location.post(url=(self.url + "/locations"), headers=self.headers, json=data)

        self.assertEqual(response.status_code, 201)

    def test_04_put_location(self):
        data = {
                "id": 2,
                "warehouse_id": "60",
                "code": "B.3.0",
                "name": "Row: B, Rack: 3, Shelf: 0",
                "created_at": "1992-05-15 03:21:32",
                "updated_at": "1992-05-15 03:21:32",
                }
        
        response = self.location.put(url=(self.url + "/locations/2"), headers=self.headers, json=data)

        self.assertEqual(response.status_code, 200)

    def test_05_delete_locations(self):
        # response_create = self.location.post(url=(self.url + "/locations"), headers=self.headers, 
        #     json={
        #         "id": 36000,
        #         "warehouse_id": "60",
        #         "code": "B.3.0",
        #         "name": "Row: B, Rack: 3, Shelf: 0",
        #         "created_at": "1992-05-15 03:21:32",
        #         "updated_at": "1992-05-15 03:21:32",
        #         }
        # )
        response = self.location.delete(url=(self.url + "/locations/600000"), headers=self.headers)
        
        self.assertEqual(response.status_code, 200)

    def test_06_unhappy_post_locations_existing_id(self):
        data = {
            "id": 1,
            "warehouse_id": 1,
            "code": "A.1.0",
            "name": "Row: A, Rack: 1, Shelf: 0",
            "created_at": "1992-05-15 03:21:32",
            "updated_at": "1992-05-15 03:21:32"
        }
        
        response = self.location.post(url=(self.url + "/locations"), headers=self.headers, json=data)

        self.assertEqual(response.status_code, 400)

    def test_07_unhappy_broken_object_post_locations(self):
        data = {
            "id": 34560,
            "warehouse_id": 1,
            "code": "A.1.0",
            "name": 2,
            "created_at": "1992-05-15 03:21:32",
            "updated_at": 1992
        }
        
        response = self.location.post(url=(self.url + "/locations"), headers=self.headers, json=data)

        self.assertEqual(response.status_code, 400)