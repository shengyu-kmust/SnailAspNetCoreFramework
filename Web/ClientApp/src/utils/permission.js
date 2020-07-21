import store from '@/store'

/**
 * @param {Array} value
 * @returns {Boolean}
 * @example see @/views/permission/directive.vue
 */
export default function checkPermission(resourceCode) {
  const roles = store.getters && store.getters.roles
  const roleNames = store.getters && store.getters.roleNames
  const resourceRoles = store.getters && store.getters.resourceRoles
  // resourceRoles 为如下格式的数组{"resourceKey": "70758034360078336","resourceName": "1","resourceCode": "1","roleKeys": []}
  // value为resourceCode，
  const isAdmin = roleNames.includes('SuperAdmin')
  if (isAdmin) {
    return true
  }
  if (resourceCode) {
    var resource = resourceRoles.find(a => (a.resourceCode || '').toLowerCase() === resourceCode.toLowerCase())
    return resource && (resource.roleKeys || []).some(a => roles.includes(a))
  }
}
