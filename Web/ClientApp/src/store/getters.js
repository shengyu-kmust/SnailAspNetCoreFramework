const getters = {
  sidebar: state => state.app.sidebar,
  size: state => state.app.size,
  device: state => state.app.device,
  visitedViews: state => state.tagsView.visitedViews,
  cachedViews: state => state.tagsView.cachedViews,
  token: state => state.user.token,
  avatar: state => state.user.avatar,
  name: state => state.user.name,
  introduction: state => state.user.introduction,
  roles: state => state.user.roles,
  roleNames: state => state.user.roleNames,
  permission_routes: state => state.permission.routes,
  errorLogs: state => state.errorLog.logs,
  resourceRoles: state => state.permission.resourceRoles,
  keyValues: state => state.keyValue.keyValues
}
export default getters
