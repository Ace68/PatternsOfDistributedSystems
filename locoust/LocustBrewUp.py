from locust import task, run_single_user
from locust import FastHttpUser


class Sales(FastHttpUser):
    host = "http://localhost:5500"
    default_headers = {
        "Accept": "*/*",
        "Accept-Encoding": "gzip, deflate, br",
        "Accept-Language": "en-US,en;q=0.5",
        "Connection": "keep-alive",
        "Cookie": "grafana_session=67a0d7596a5adb9c65234ab3b2982d68; grafana_session_expiry=1709305914",
        "Host": "localhost:5500",
        "Referer": "http://localhost:5500/documentation/index.html",
        "Sec-Fetch-Dest": "empty",
        "Sec-Fetch-Mode": "cors",
        "Sec-Fetch-Site": "same-origin",
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:123.0) Gecko/20100101 Firefox/123.0",
    }

    @task
    def t(self):
        with self.rest("GET", "/v1/sales") as resp:
            pass

    @task
    def t(self):
        with self.rest("GET", "/v1/sales/beers") as resp:
            pass

class Warehouses(FastHttpUser):
    host = "http://localhost:5600"
    default_headers = {
        "Accept": "*/*",
        "Accept-Encoding": "gzip, deflate, br",
        "Accept-Language": "en-US,en;q=0.5",
        "Connection": "keep-alive",
        "Cookie": "grafana_session=67a0d7596a5adb9c65234ab3b2982d68; grafana_session_expiry=1709305914",
        "Host": "localhost:5500",
        "Referer": "http://localhost:5600/documentation/index.html",
        "Sec-Fetch-Dest": "empty",
        "Sec-Fetch-Mode": "cors",
        "Sec-Fetch-Site": "same-origin",
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:123.0) Gecko/20100101 Firefox/123.0",
    }

    @task
    def t(self):
        with self.rest("GET", "/v1/wareHouses/availabilities/") as resp:
            pass


if __name__ == "__main__":
    run_single_user(Sales)
    # run_single_user(Warehouses)
