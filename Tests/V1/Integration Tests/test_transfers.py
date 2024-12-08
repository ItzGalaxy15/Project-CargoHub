import unittest
import httpx

def checkTransfersByID(transfers):
    if transfers.get("id") == 1:
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

    if len(item) != 18:
        return False
    return True

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
                self.assertTrue(check_transfer(transfer))

    def test_02_get_transfer(self):
        response = self.client.get(url=(self.url + "/transfers/1"), headers=self.headers)
        self.assertEqual(response.status_code, 200)
        self.assertEqual(type(response.json()), dict)
        self.assertTrue(check_transfer(response.json()))

    def test_03_post_transfer(self):
        new_transfer = {
            "id": 9,
            "reference": "TR00007",
            "transfer_from": 1,
            "transfer_to": 1,
            "transfer_status": "Completed",
            "created_at": "2000-03-11T13:11:14Z",
            "updated_at": "2000-03-12T16:11:14Z",
            "items": [
                {
                    "item_id": "P007435",
                    "amount": 23
                }
            ]
        }
        response = self.client.post(url=(self.url + "/transfers"), headers=self.headers, json=new_transfer)
        self.assertEqual(response.status_code, 201)
        self.assertTrue(check_transfer(response.json()))

    def test_04_put_transfer(self):
        updated_transfer = {
            "id": 2,
            "reference": "TV00007",
            "transfer_from": 842,
            "transfer_to": 9229,
            "transfer_status": "Not completed",
            "created_at": "2000-03-11T13:11:14Z",
            "updated_at": "2000-03-12T16:11:14Z",
            "items": [
                {
                    "item_id": "P007435",
                    "amount": 23
                }
            ]
        }
        response = self.client.put(url=(self.url + "/transfers/2"), headers=self.headers, json=updated_transfer)
        self.assertEqual(response.status_code, 200)
        # self.assertTrue(check_transfer(response.json()))

    def test_delete_transfer(self):
        response = self.client.delete(url=(self.url + "/transfers/9"), headers=self.headers)
        self.assertEqual(response.status_code, 200)

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

def check_transfer(transfer):
    required_keys = ["id", "reference", "transfer_from", "transfer_to", "transfer_status", "created_at", "updated_at", "items"]
    for key in required_keys:
        if key not in transfer:
            return False
    return True