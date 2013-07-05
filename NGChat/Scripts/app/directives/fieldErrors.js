'use strict';

angular
    .module('chat.directives')
    .directive('fieldErrors', ['$rootScope', function ($rootScope) {
        return {
            replace: true,
            restrict: 'EA',
            scope: true,
            template:
                '<div class="fieldErrors" data-ng-show="showErrors">' +
                    '<span class="help-block" data-ng-repeat="error in serverErrors">{{ error }}</span>' +
                    '<span class="help-block" data-ng-repeat="error in clientErrors" data-ng-show="error.show">' +
                        '{{ error.message }}' +
                    '</span>' +
                '</div>',
            link: function (scope, element, attrs) {
                var form = angular.element(element).parents('form').inheritedData('$formController'),
                    clientErrorTypes = scope.$eval(attrs.clientErrors);

                scope.serverErrors = [];
                scope.clientErrors = [];
                scope.showErrors = false;

                scope.$watch('showErrors', function (showErrors) {
                    form[attrs.fieldErrors]._hasAnyError = showErrors;
                });

                for (var type in clientErrorTypes) {
                    var error = {
                        type: type,
                        message: clientErrorTypes[type],
                        show: false
                    };

                    scope.clientErrors.push(error);

                    scope.$watch(function () {
                        return form[attrs.fieldErrors].$error[type] && form[attrs.fieldErrors].$dirty;
                    }, function (showError) {
                        if (showError) {
                            error.show = true;
                            scope.showErrors = true;
                        } else {
                            error.show = false;

                            if (scope.serverErrors.length == 0)
                                scope.showErrors = false;
                        }
                    });
                }

                scope.$watch(attrs.serverErrors, function (errors) {
                    angular.forEach(errors, function (error, i) {
                        if (error.key == attrs.fieldErrors && angular.isArray(error.messages) && error.messages.length > 0) {
                            scope.serverErrors = error.messages;
                            return;
                        }
                    });

                    if (scope.serverErrors.length > 0)
                        scope.showErrors = true;
                    else if (scope.clientErrors.length == 0)
                        scope.showErrors = false;
                });
            }
        };
    }]);
