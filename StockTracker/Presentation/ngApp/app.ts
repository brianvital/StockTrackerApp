﻿namespace StockTracker {
    
    angular.module('StockTracker', ['ngRoute']);

    angular.module('StockTracker').factory('authInterceptor',
        ($q: ng.IQService, $window: ng.IWindowService, $location: ng.ILocationService) => {
            return {
                request: (config) => {
                    config.headers = config.headers || {};
                    let token = $window.localStorage.getItem('token');
                    if (token) {
                        config.headers.Authorization = `Bearer ${token}`;
                    }
                    return config;
                },
                responseError: (response) => {
                    if (response.status === 401) {
                        $location.path('/login');
                    }
                    return $q.reject(response);
                }
            };
        });

    angular.module('StockTracker')
        .config(function ($routeProvider: ng.route.IRouteProvider, $httpProvider: ng.IHttpProvider) {

            $httpProvider.interceptors.push('authInterceptor');

            $routeProvider.when('/', { template: 'Hello World!' });
            $routeProvider.when('/login', {
                templateUrl: '/Presentation/ngApp/views/login.html',
                controller: StockTracker.Controllers.AuthController,
                controllerAs: 'controller'
            });
            $routeProvider.when('/register', {
                templateUrl: '/Presentation/ngApp/views/register.html',
                controller: StockTracker.Controllers.AuthController,
                controllerAs: 'controller'
            });
            $routeProvider.when('/research', {
                templateUrl: '/Presentation/ngApp/views/research.html',
                controller: StockTracker.Controllers.AuthController,
                controllerAs: 'controller'
            });
            $routeProvider.when('/research', {
                templateUrl: '/Presentation/ngApp/views/research.html',
                controller: StockTracker.Controllers.ResearchController,
                controllerAs: 'controller'
            });
            $routeProvider.when('/ticker/:ticker', {
                templateUrl: '/Presentation/ngApp/views/stockDetails.html',
                controller: StockTracker.Controllers.StockDetailsController,
                controllerAs: 'controller'
            });

        });
}