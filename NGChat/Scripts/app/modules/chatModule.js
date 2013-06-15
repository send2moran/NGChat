'use strict';

angular
    .module('chat')
    .config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
        var resolves = {
            auth: ['$q', '$location', 'userFactory', 'enumFactory', function ($q, $location, userFactory, enumFactory) {
                var delay = $q.defer(),
                    isHomepage = $location.url() === '/';

                console.log('user.authState in resolver: ' + userFactory.authState);

                if (userFactory.authState === enumFactory.authState.unknown) {
                    userFactory.checkIfLogged()
                        .success(function (data, status, headers, config) {
                            if (data.success) {
                                if ($location.url() !== '/chat') {
                                    delay.reject();
                                    $location.path('/chat');
                                } else
                                    delay.resolve();
                            } else {
                                if (!isHomepage) {
                                    delay.reject();
                                    $location.path('/');
                                } else
                                    delay.resolve();
                            }
                        })
                        .error(function (data, status, headers, config) {
                            if (!isHomepage) {
                                delay.reject();
                                $location.path('/');
                            } else
                                delay.resolve();
                        });
                } else if (userFactory.authState === enumFactory.authState.notAuthenticated) {
                    delay.resolve();

                    if (!isHomepage)
                        $location.path('/');
                } else if (userFactory.authState === enumFactory.authState.authenticated) {
                    if (isHomepage) {
                        delay.reject();
                        $location.path('/chat');
                    } else
                        delay.resolve();
                }

                return delay.promise;
            }]
        };

        $routeProvider
            .when('/', {
                controller: 'LogInController',
                templateUrl: '/Home/LogIn',
                resolve: {
                    auth: resolves.auth
                }
            })
            .when('/chat', {
                controller: 'HomeController',
                templateUrl: '/Home/Main',
                resolve: {
                    auth: resolves.auth
                }
            })
            .otherwise({ redirectTo: '/' });

        $locationProvider.hashPrefix('!');
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
