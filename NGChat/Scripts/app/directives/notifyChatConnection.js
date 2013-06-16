'use strict';

angular
    .module('chat.directives')
    .directive('notifyChatConnection', ['$rootScope', '$timeout', 'enumFactory', function ($rootScope, $timeout, enumFactory) {
        return {
            replace: false,
            restrict: 'EA',
            scope: true,
            template: '<div class="chatConnectionNotify progress">' +
                          '<div class="bar" style="width: 100%">{{ notifyText }}</div>' +
                      '</div>',
            link: function (scope, element, attrs) {
                var notifyElem = element.find('.chatConnectionNotify'),
                    TEXT_CONNECTING = 'Łączenie się z czatem, proszę czekać...',
                    TEXT_CONNECTED = 'Możesz zacząć czatować.',
                    TEXT_RECONNECTING_OR_DISCONNECTED = 'Połączenie zostało przerwane. Próba ponownego połączenia...';

                scope.notifyText = '';

                function tryShowNotifyBar() {
                    if (notifyElem.is(':hidden'))
                        notifyElem.fadeIn();
                };

                function setNotifyElemInfoStyle() {
                    notifyElem.attr('class', 'chatConnectionNotify progress progress-info progress-striped active');
                };

                function resetNotifyElemStyle() {
                    notifyElem.attr('class', 'chatConnectionNotify progress');
                };

                function setNotifyElemErrorStyle() {
                    notifyElem.attr('class', 'chatConnectionNotify progress progress-danger progress-striped active');
                };

                scope.$watch(attrs.notifyChatConnection, function (newValue, oldValue) {
                    switch (newValue) {
                        case enumFactory.connectionState.connecting:
                            console.log('notifyChatConnection > connecting');

                            setNotifyElemInfoStyle();
                            scope.notifyText = TEXT_CONNECTING;
                            tryShowNotifyBar();

                            break;
                        case enumFactory.connectionState.connected:
                            console.log('notifyChatConnection > connected');

                            // defer hiding info, becouse normally connect is very quick
                            // and only flash of element is visible
                            $timeout(function () {
                                resetNotifyElemStyle();
                                notifyElem.addClass('progress-success');
                                scope.notifyText = TEXT_CONNECTED;

                                $timeout(function () {
                                    notifyElem.fadeOut(function () {
                                        scope.notifyText = '';
                                    });
                                }, 2000);
                            }, 500);

                            break;
                        case enumFactory.connectionState.reconnecting:
                            console.log('notifyChatConnection > reconnecting');

                            scope.notifyText = TEXT_RECONNECTING_OR_DISCONNECTED;
                            setNotifyElemErrorStyle();
                            tryShowNotifyBar();

                            break;
                        case enumFactory.connectionState.disconnected:
                            console.log('notifyChatConnection > disconnected');

                            scope.notifyText = TEXT_RECONNECTING_OR_DISCONNECTED;
                            setNotifyElemErrorStyle();
                            tryShowNotifyBar();

                            break;
                    }
                });

            }
        };
    }]);
