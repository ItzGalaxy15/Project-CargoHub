import unittest
import httpx

class TestTransfers(unittest.TestCase):
    def setUp(self):
        self.client = httpx
        self.url = "http://localhost:3000/api/v1"
        self.headers = httpx.Headers({'API_KEY': 'a1b2c3d4e5'})

    def test_01_get_transfers(self):
        response = self.client.get(url=(self.url + "/transfers"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), list)
        if len(response.json()) > 0:
            for transfer in response.json():
                self.assertTrue(self.check_transfer(transfer))

    def test_02_get_transfer(self):
        response = self.client.get(url=(self.url + "/transfers/1"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), dict)
        self.assertTrue(self.check_transfer(response.json()))

    def test_03_post_transfer(self):
        new_transfer = {
            "id": 13,
            "source_id": 1,
            "destination_id": 2,
            "items": [{"item_id": 1, "amount": 10}],
            "transfer_status": "Scheduled"
        }
        response = self.client.post(url=(self.url + "/transfers"), headers=self.headers, json=new_transfer)
        self.assertEqual(response.status_code, 201)
        self.assertTrue(self.check_transfer(response.json()))

    def test_04_put_transfer(self):
        updated_transfer = {
            "id": 1,
            "source_id": 1,
            "destination_id": 2,
            "items": [{"item_id": 1, "amount": 20}],
            "transfer_status": "In Progress"
        }
        response = self.client.put(url=(self.url + "/transfers/1"), headers=self.headers, json=updated_transfer)
        self.assertEqual(response.status_code, 200)
        self.assertTrue(self.check_transfer(response.json()))

    def test_delete_transfer(self):
        response = self.client.delete(url=(self.url + "/transfers/13"), headers=self.headers)
        self.assertEqual(response.status_code, 204)

#####################################################################################################################################
    # unhappy test transfer -> existing id, invalid structure
    def test_05_post_transfer_existing_id(self):
        existing_transfer = {
            "id": 1,  # Assuming 1 already exists
            "source_id": 1,
            "destination_id": 2,
            "items": [{"item_id": 1, "amount": 10}],
            "transfer_status": "Scheduled"
        }
        response = self.client.post(url=(self.url + "/transfers"), headers=self.headers, json=existing_transfer)
        self.assertEqual(response.status_code, 400)  # Assuming 400 Bad Request for duplicate ID


    def test_06_post_transfer_invalid_structure(self):
        invalid_transfer = {
            "id": 2,
            "source_id": 1,
            # Missing destination_id and other required fields
            "items": [{"item_id": 1, "amount": 10}],
            "transfer_status": "Scheduled"
        }
        response = self.client.post(url=(self.url + "/transfers"), headers=self.headers, json=invalid_transfer)
        self.assertEqual(response.status_code, 400)  # Assuming 400 Bad Request for invalid structure

######################################################################################################################################

    def check_transfer(self, transfer):
        required_keys = ["id", "source_id", "destination_id", "items", "transfer_status", "created_at", "updated_at"]
        for key in required_keys:
            if key not in transfer:
                return False
        return True

if __name__ == '__main__':
    unittest.main()