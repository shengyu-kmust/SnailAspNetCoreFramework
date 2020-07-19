import checkPermission from '@/utils/permission'

export default {
  inserted(el, binding, vnode) {
    const { value } = binding
    if (value) {
      if (!checkPermission(value)) {
        el.parentNode && el.parentNode.removeChild(el)
      } else {
        return true
      }
    }
  }
}
