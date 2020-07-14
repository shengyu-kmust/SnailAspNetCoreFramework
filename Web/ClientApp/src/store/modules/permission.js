import { asyncRoutes, constantRoutes } from '@/router' // asyncRoutes为要进行权限控制的路由，constantRoutes为不用权限的路由
import { getAllResourceRoles } from '@/api/user'

/**
 * 不用这个
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
 * 计算用户的可使用路由
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

/**
 * 设置路由信息中的角色
 * @param {*} routes
 * @param {*} resourceRoles
 */
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
  routes: [], // 用户访问的所有路由，为addRoutes和constantRoutes合集
  addRoutes: [], // 用户可访问的所有动态权限路由
  resourceRoles: [] // // 为如下格式的数组{"resourceKey": "70758034360078336","resourceName": "1","resourceCode": "1","roleKeys": []}
}

const mutations = {
  // 设置用户可访问的所有路由
  SET_ROUTES: (state, routes) => {
    state.addRoutes = routes
    state.routes = constantRoutes.concat(routes)
  },
  SET_RESOURCE_ROLES: (state, resourceRoles) => {
    state.resourceRoles = resourceRoles
  }
}

const actions = {
  // 生成用户可访问的所有路由
  generateRoutes({ commit, state }, roles, isAdmin = false) {
    return new Promise(resolve => {
      let accessedRoutes
      if (isAdmin) {
        // 超级管理员能访问所有路由
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
