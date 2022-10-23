import 'bootstrap/dist/css/bootstrap.css';
import React from "react";
import App from "./App";
import { BrowserRouter } from 'react-router-dom';
import { createRoot } from 'react-dom/client';
import { Auth0Provider } from "@auth0/auth0-react";
import * as serviceWorkerRegistration from './serviceWorkerRegistration';
import reportWebVitals from './reportWebVitals';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');
const root = createRoot(rootElement);

root.render(
    <Auth0Provider
        domain="dev-lajrziuc.us.auth0.com"
        clientId="g2tDObZINz3vJXu5w2aTBTQfPIoIDgwy"
        redirectUri={window.location.origin}
    >
        <BrowserRouter basename={baseUrl}>
            <App />
        </BrowserRouter>
    </Auth0Provider>
);
serviceWorkerRegistration.unregister();
reportWebVitals();

/**import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import * as serviceWorkerRegistration from './serviceWorkerRegistration';
import reportWebVitals from './reportWebVitals';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');
const root = createRoot(rootElement);

root.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
serviceWorkerRegistration.unregister();

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();*/
