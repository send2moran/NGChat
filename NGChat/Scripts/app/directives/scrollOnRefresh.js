'use strict';

angular
    .module('chat.directives')
    .directive('scrollOnRefresh', function () {
        return function (scope, element, attrs) {
                scope.$watch(attrs.scrollOnRefresh, function (newValue, oldValue) {
                    if (newValue && newValue.length > 0) {
                        element[0].scrollTop = element[0].scrollHeight;
                    }
                }, true);
            }
    });