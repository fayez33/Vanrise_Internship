app.service('apiService', function ($http) {
    var baseUrl = '/api/'; // Adjust this if your local IIS uses a specific port

    this.getDevices = function () {
        return $http.get(baseUrl + 'Devices/GetAll');
    };

    this.searchDevices = function (name) {
        // Appends the query string for the API endpoint
        var url = baseUrl + 'Devices/Search' + (name ? '?name=' + encodeURIComponent(name) : '');
        return $http.get(url);
    };

    this.addDevice = function (device) {
        return $http.post(baseUrl + 'Devices/Add', device);
    };

    this.updateDevice = function (device) {
        return $http.post(baseUrl + 'Devices/Update', device);
    };
    this.deleteDevice = function (id) {
        return $http.delete(baseUrl + 'Devices/Delete?id=' + id);
    };
    // --- Client API Calls ---
    this.getClients = function () {
        return $http.get(baseUrl + 'Clients/GetAll');
    };

    this.searchClients = function (name, type) {
        var url = baseUrl + 'Clients/Search?';
        if (name) url += 'name=' + encodeURIComponent(name) + '&';
        // Check if type is selected (not null or empty string)
        if (type !== undefined && type !== null && type !== '') url += 'type=' + type;
        return $http.get(url);
    };

    this.addClient = function (client) {
        return $http.post(baseUrl + 'Clients/Add', client);
    };

    this.updateClient = function (client) {
        return $http.post(baseUrl + 'Clients/Update', client);
    };

    this.deleteClient = function (id) {
        return $http.delete(baseUrl + 'Clients/Delete?id=' + id);
    };
    // --- Phase 2: Client API Calls ---
    this.getClients = function () {
        return $http.get(baseUrl + 'Clients/GetAll');
    };

    this.searchClients = function (name, type) {
        var url = baseUrl + 'Clients/Search?';
        if (name) url += 'name=' + encodeURIComponent(name) + '&';
        // Ensure type is only appended if it has a value (0 or 1)
        if (type !== undefined && type !== null && type !== '') url += 'type=' + type;
        return $http.get(url);
    };

    this.addClient = function (client) {
        return $http.post(baseUrl + 'Clients/Add', client);
    };

    this.updateClient = function (client) {
        return $http.post(baseUrl + 'Clients/Update', client);
    };

    this.deleteClient = function (id) {
        return $http.delete(baseUrl + 'Clients/Delete?id=' + id);
    };
    // --- Phase 2: Phone Numbers API Calls ---
    this.getPhoneNumbers = function () {
        return $http.get(baseUrl + 'PhoneNumbers/GetAll');
    };

    this.searchPhoneNumbers = function (number, deviceId) {
        var url = baseUrl + 'PhoneNumbers/Search?';
        if (number) url += 'number=' + encodeURIComponent(number) + '&';
        if (deviceId !== undefined && deviceId !== null && deviceId !== '') url += 'deviceId=' + deviceId;
        return $http.get(url);
    };

    this.addPhoneNumber = function (phone) {
        return $http.post(baseUrl + 'PhoneNumbers/Add', phone);
    };

    this.updatePhoneNumber = function (phone) {
        return $http.post(baseUrl + 'PhoneNumbers/Update', phone);
    };

    this.deletePhoneNumber = function (id) {
        return $http.delete(baseUrl + 'PhoneNumbers/Delete?id=' + id);
    };
    this.getActiveClientPhoneNumbers = function (clientId) {
        return $http.get(baseUrl + 'PhoneNumbers/GetActiveByClient?clientId=' + clientId);
    };
    // --- Phase 2: Reservations API Calls ---
    this.getReservations = function () {
        return $http.get(baseUrl + 'Reservations/GetAll');
    };

    this.searchReservations = function (clientId, phoneNumberId) {
        var url = baseUrl + 'Reservations/Search?';
        if (clientId !== undefined && clientId !== null && clientId !== '') url += 'clientId=' + clientId + '&';
        if (phoneNumberId !== undefined && phoneNumberId !== null && phoneNumberId !== '') url += 'phoneNumberId=' + phoneNumberId;
        return $http.get(url);
    };
    // --- Phase 2: Reservations API Calls ---
    this.getReservations = function () {
        return $http.get(baseUrl + 'Reservations/GetAll');
    };

    this.searchReservations = function (clientId, phoneNumberId) {
        var url = baseUrl + 'Reservations/Search?';
        if (clientId !== undefined && clientId !== null && clientId !== '') url += 'clientId=' + clientId + '&';
        if (phoneNumberId !== undefined && phoneNumberId !== null && phoneNumberId !== '') url += 'phoneNumberId=' + phoneNumberId;
        return $http.get(url);
    };
    this.reservePhoneNumber = function (request) {
        return $http.post(baseUrl + 'Reservations/Reserve', request);
    };

    this.unreservePhoneNumber = function (request) {
        return $http.post(baseUrl + 'Reservations/Unreserve', request);
    };

    // --- Phase 3: Reports API Calls ---
    this.getClientsPerTypeReport = function (type) {
        var url = baseUrl + 'Reports/ClientsPerType';
        if (type !== undefined && type !== null && type !== '') {
            url += '?type=' + type;
        }
        return $http.get(url);
    };

    this.getPhoneStatusReport = function (deviceId, isReserved) {
        var url = baseUrl + 'Reports/PhoneStatus?';
        if (deviceId !== undefined && deviceId !== null && deviceId !== '') {
            url += 'deviceId=' + deviceId + '&';
        }
        if (isReserved !== undefined && isReserved !== null && isReserved !== '') {
            url += 'isReserved=' + isReserved;
        }
        return $http.get(url);
    };
    // --- Phase 3: Reports API Calls ---
    this.getClientsPerTypeReport = function (type) {
        var url = baseUrl + 'Reports/ClientsPerType';

        // Add the filter to the URL if the user selected one
        if (type !== undefined && type !== null && type !== '') {
            url += '?type=' + type;
        }
        return $http.get(url);
    };

    this.getPhoneStatusReport = function (deviceId, isReserved) {
        var url = baseUrl + 'Reports/PhoneStatus?';

        // Add the device filter if selected
        if (deviceId !== undefined && deviceId !== null && deviceId !== '') {
            url += 'deviceId=' + deviceId + '&';
        }

        // Add the status filter if selected
        if (isReserved !== undefined && isReserved !== null && isReserved !== '') {
            url += 'isReserved=' + isReserved;
        }
        return $http.get(url);
    };
    // --- Phase 4: Authentication API Calls ---
    this.login = function (credentials) {
        return $http.post(baseUrl + 'Login/Authenticate', credentials);
    };
    this.register = function (credentials) {
        return $http.post(baseUrl + 'Login/Register', credentials);
    };
});