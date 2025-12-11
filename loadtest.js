import http from 'k6/http';
import { check, sleep } from 'k6';
export let options = {
    vus: 100,          // 100 virtual users
    duration: '30s',   // run for 30 seconds
};
const url = 'https://bp-calculator-qa-cbgkg0bfdrdbf4c8.norwayeast-01.azurewebsites.net/';
export default function () {
    // GET the calculator page
    let res1 = http.get(url);
    check(res1, {
        'Loaded calculator page': (r) => r.status === 200,
    });
    // POST the form (simulate a user submitting BP values)
    const payload = {
        "BP.Systolic": "110",
        "BP.Diastolic": "70"
    };
    const headers = {
        'Content-Type': 'application/x-www-form-urlencoded',
    };
    let res2 = http.post(url, payload, { headers });
    check(res2, {
        'Submitted form successfully': (r) => r.status === 200,
        'Contains Ideal': (r) => r.body.includes("Ideal"),
    });
    sleep(1);
}