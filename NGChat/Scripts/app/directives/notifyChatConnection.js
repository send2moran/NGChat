'use strict';

angular
    .module('chat.directives')
    .directive('notifyChatConnection', ['$rootScope', '$timeout', 'enumFactory', function ($rootScope, $timeout, enumFactory) {
        return {
            replace: false,
            restrict: 'EA',
            scope: true,
            template: '<div class="chatConnectionNotify progress progress-info progress-striped active">' +
                          '<div class="bar" style="width: 100%">{{ notifyText }}</div>' +
                      '</div>',
            link: function (scope, element, attrs) {
                scope.notifyText = '';

                var TEXT_IN_PROGRESS = 'Łączenie się z czatem, proszę czekać...';
                var TEXT_SUCCESS = 'Możesz zacząć czatować.';

                scope.$watch(attrs.notifyChatConnection, function (newValue, oldValue) {
                    if (newValue === enumFactory.connectionState.inProgress) {
                        scope.notifyText = TEXT_IN_PROGRESS;
                        element.find('.chatConnectionNotify').fadeIn();
                    } else if (newValue === enumFactory.connectionState.connected) {
                        // defer hiding info, becouse normally connect is very quick
                        // and only flash of element is visible
                        $timeout(function () {
                            var notifyElem = element.find('.chatConnectionNotify');
                            notifyElem
                                .removeClass('progress-info')
                                .removeClass('active')
                                .removeClass('progress-striped')
                                .addClass('progress-success');

                            scope.notifyText = TEXT_SUCCESS;

                            $timeout(function () {
                                notifyElem.fadeOut(function () {
                                    scope.notifyText = '';
                                    notifyElem
                                        .addClass('progress-info')
                                        .addClass('active')
                                        .addClass('progress-striped')
                                        .removeClass('progress-success')
                                });
                            }, 2000);
                        }, 800);
                    }
                });

            }
        };
    }]);
