import { asyncRoutes, constantRoutes } from '@/router'
import { getAllResourceRoles } from '@/api/user'

/**
 * Use meta.role to determine if the current user has permission
 * @param roles
 * @param route
 */
function hasPermission(roles, route) {
  if (route.meta && route.meta.roles) {
    return roles.some(role => route.meta.roles.includes(role))
  } else {
    return true
  }
}

/**
 * Filter asynchronous routing tables by recursion
 * @param routes asyncRoutes
 * @param roles
 */
export function filterAsyncRoutes(routes, roles) {
  const res = []
  if (!routes || routes.length === 0) {
    routes = []
  }
  routes.forEach(route => {
    const tmp = { ...route }
    if (hasPermission(roles, tmp)) {
      if (tmp.children) {
        tmp.children = filterAsyncRoutes(tmp.children, roles)
      }
      res.push(tmp)
    }
  })

  return res
}

export function setRouteRoles(routes, resourceRoles) {
  if (routes && resourceRoles) {
    routes.forEach(route => {
      var thisRouteResourceRoles = resourceRoles.find(a => a.resourceCode === route.meta.resourceCode)
      if (thisRouteResourceRoles) {
        route.meta.roles = thisRouteResourceRoles.roleKeys
      }
    })
  }
}

const state = {
  routes: [],
  addRoutes: [],
  resourceRoles: []
}

const mutations = {
  SET_ROUTES: (state, routes) => {
    state.addRoutes = routes
    state.routes = constantRoutes.concat(routes)
  },
  SET_RESOURCE_ROLES: (state, resourceRoles) => {
    state.resourceRoles = resourceRoles
  }
}

const actions = {
  generateRoutes({ commit, state }, roles, isAdmin = false) {
    return new Promise(resolve => {
      let accessedRoutes
      if (isAdmin) {
        accessedRoutes = asyncRoutes || []
      } else {
        setRouteRoles(asyncRoutes, state.resourceRoles)
        accessedRoutes = filterAsyncRoutes(asyncRoutes, roles)
      }
      commit('SET_ROUTES', accessedRoutes)
      resolve(accessedRoutes)
    })
  },
  getAllResourceRoles({ commit }) {
    return new Promise((resolve, reject) => {
      getAllResourceRoles().then(res => {
        commit('SET_RESOURCE_ROLES', res.data)
        resolve(res.data)
      }).catch(error => {
        reject(error)
      })
    })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
