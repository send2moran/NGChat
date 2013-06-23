'use strict';

angular
    .module('chat.services')
    .factory('userFactory', ['$http', '$rootScope', '$q', '$timeout', 'enumFactory', function ($http, $rootScope, $q, $timeout, enumFactory) {
        var factory = {
            user: null,
            authState: enumFactory.authState.unknown
        };
        
        factory.initUserObject = function (id, name) {
            return {
                id: id || '',
                name: name || ''
            };
        };
        
        factory.login = function (username) {
            return $http.post('/user/login', { username: username })
                .success(function (data, status, headers, config) {
                    if (data && data.success) {
                        factory.user = factory.initUserObject(data.model.id, data.model.name);
                        factory.authState = enumFactory.authState.authenticated;
                    }
                });
        };

        factory.logout = function () {
            return $http.get('/user/logout')
                .success(function (data) {
                    if (data && data.success) {
                        factory.user = null;
                        factory.authState = enumFactory.authState.notAuthenticated;
                    }
                });
        };

        factory.checkIfLogged = function () {
            return $http.get('/User/CheckIfLogged')
                .success(function (data, status, headers, config) {
                    if (data.success) {
                        factory.authState = enumFactory.authState.authenticated;
                        factory.user = factory.initUserObject(data.model.id, data.model.name);
                    } else
                        factory.authState = enumFactory.authState.notAuthenticated;
                })
                .error(function (data, status, headers, config) {
                    factory.authState = enumFactory.authState.notAuthenticated;
                });
        };

        return factory;
    }]);
