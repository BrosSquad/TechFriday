import { check } from 'k6';
import http from 'k6/http';

export const options = {
  vus: 10,
  duration: '1m',
  insecureSkipTLSVerify: true,
};

export default function () {
  const url = 'http://localhost:5241/auth/login';

  const payload = JSON.stringify({
    email: 'test',
    password: 'password',
  });

  const params = {
    headers: {
      'Content-Type': 'application/json',
    },
  };

  const res = http.post(url, payload, params);

  check(res, {
    'is status 422': (r) => r.status === 422,
  });
}
