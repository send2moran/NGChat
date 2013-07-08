'use strict';

angular
    .module('chat.directives')
    .directive('scrollOnRefresh', function () {
        return {
            scope: {
                enabled: '=scrollOnRefreshEnabled',
                messages: '=scrollOnRefresh'
            },
            link: function (scope, element, attrs) {
                scope.$watch('messages', function (newValue, oldValue) {
                    if (newValue && newValue.length > 0 &&
                        (scope.enabled === undefined || scope.enabled === true)) {
                        element[0].scrollTop = element[0].scrollHeight;
                    }
                }, true);
            }
        }
    });