<script>
  export default {
    name: 'MenuItem',
    functional: true,
    props: {
      icon: {
        type: String,
        default: ''
      },
      title: {
        type: String,
        default: ''
      },
      iconClass: {
        type: String,
        default: ''
      }
    },
    render(h, context) {
      const { icon, title, iconClass } = context.props
      const vnodes = []
      if (iconClass) {
        vnodes.push(<li class={iconClass} style='margin-right:16px;' />)
      }
      if (icon) {
        vnodes.push(<svg-icon icon-class={icon} />)
      }

      if (title) {
        vnodes.push(<span slot='title'>{(title)}</span>)
      }

      return vnodes
    }
  }
</script>
