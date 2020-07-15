import { allRoute } from '@/router' // asyncRoutes为要进行权限控制的路由，constantRoutes为不用权限的路由
import { getAllResourceRoles } from '@/api/user'

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
  SET_RESOURCE_ROLES: (state, resourceRoles) => {
    state.resourceRoles = resourceRoles
  }
}

const actions = {
  // 生成用户可访问的所有路由
  generateRoutes({ commit, state }, roles, isAdmin = false) {
    return new Promise(resolve => {
      setRouteRoles(allRoute, state.resourceRoles)
      resolve(allRoute)
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
