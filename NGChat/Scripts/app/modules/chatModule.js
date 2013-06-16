'use strict';

angular
    .module('chat')
    .config(['$routeProvider', function ($routeProvider) {
        var resolvers = {
            auth: ['$q', '$location', 'userFactory', 'enumFactory', function ($q, $location, userFactory, enumFactory) {
                var delay = $q.defer(),
                    isHomepage = $location.url() === '/',
                    isChat = $location.url() === '/chat';

                function redirectIf(condition, redirectPath) {
                    if (condition) {
                        delay.reject();
                        $location.path(redirectPath);
                    } else
                        delay.resolve();
                };

                console.log('user.authState in resolver: ' + userFactory.authState);

                switch (userFactory.authState) {
                    case enumFactory.authState.unknown:
                        userFactory.checkIfLogged()
                            .success(function (data, status, headers, config) {
                                if (data.success)
                                    redirectIf(!isChat, '/chat');
                                else
                                    redirectIf(!isHomepage, '/');
                            })
                            .error(function (data, status, headers, config) {
                                redirectIf(!isHomepage, '/');
                            });

                        break;
                    case enumFactory.authState.notAuthenticated:
                        redirectIf(!isHomepage, '/');

                        break;
                    case enumFactory.authState.authenticated:
                        redirectIf(!isChat, '/chat');

                        break;
                }

                return delay.promise;
            }]
        };

        $routeProvider
            .when('/', {
                controller: 'LogInController',
                templateUrl: '/Home/LogIn',
                resolve: {
                    auth: resolvers.auth
                }
            })
            .when('/chat', {
                controller: 'HomeController',
                templateUrl: '/Home/Main',
                resolve: {
                    auth: resolvers.auth
                }
            })
            .otherwise({ redirectTo: '/' });
    }])
    .run(['$location', '$q', '$timeout', '$rootScope', 'userFactory', function ($location, $q, $timeout, $rootScope, userFactory) {
        $rootScope.$on("$routeChangeStart", function (current, next) {
            console.debug('$routeChangeStart: ' + next.controller);
        });

        $rootScope.$on("$routeChangeSuccess", function (current, next) {
            console.debug('$routeChangeSuccess: ' + next.controller);
        });

        $rootScope.$on("$routeChangeError", function (current, next) {
            console.debug('$routeChangeError: ' + next.controller);
        });
    }]);
