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
                notConnected: 2,
                inProgress: 3,
                connected: 4
            }
        };

        return factory;
    }]);