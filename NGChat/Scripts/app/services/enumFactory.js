'use strict';

angular
    .module('chat.services')
    .factory('enumFactory', [function () {
        var factory = {
            authState: {
                unknown: 1,
                notAuthenticated: 2,
                authenticated: 3
            },
            connectionState: {
                none: 1,
                disconnected: 2,
                connecting: 3,
                connected: 4,
                reconnecting: 5
            }
        };

        return factory;
    }]);