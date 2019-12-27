<!--
 /**
  *
  * 侧边栏弹出组件
  *
  * author: lanbo
  *
  * version: 0.5
  *
  * description: 用于高级筛选等场景
  *
  * props:
  *     visible: Boolean | true: 显示, false: 隐藏 | 支持 .sync 修饰符
  *     top: Number | 上边距（不设置默认是头部 + tab高度）
  *     left: Number | 左边距（不设置默认根据内容宽度自动设置），从左侧菜单栏开始算起（0则与菜单栏对齐）
  *     close-on-click-modal: Boolean | true: 可以通过点击背景遮罩隐藏, false: 点击无效
  *     modal: Boolean | true: 显示遮罩, false: 不显示遮罩
  *
  * events:
  *     open 显示时触发
  *     close 隐藏时触发
  *
  * changelog
  *     v0.1 2018-11-13 by lanbo | 基本功能
  *     v0.2 2018-11-19 by lanbo | 增加 left prop
  *     v0.3 2018-11-20 by lanbo | 没有 modal 时增加阴影效果
  *     v0.4 2018-11-28 by lanbo | v-lading 指令显示修改，tab页切换时隐藏和显示调整
  *     v0.5 2019-01-22 by lanbo | 修复因为 VNode 重新生成，让 $el 插入到父组件 DOM 中，导致卸载时 document.body 取不到 $el 而报错的问题
  *
  */
-->
<template>
    <div class="ex-drawer" v-show="isActive" v-loading="true">
        <div class="ex-drawer__content" :class="classList" :style="style">
            <div class="ex-drawer__content--sidebar" @click="handleSidebarClick">
                <div class="i fa fa-indent"></div>
            </div>
            <div class="ex-drawer__content--main">
                <slot name="header"></slot>
                <slot name="body"></slot>
            </div>
        </div>
        <div v-show="visibleVal" v-if="modal" class="ex-drawer__shade" @click="handleShadeClick"></div>
    </div>
</template>

<script>
export default {
    name: 'ExDrawer',
    props: {
        visible: {
            type: Boolean,
            default: false
        },
        top: {
            type: [Number, Boolean],
            default: false
        },
        left: {
            type: [Number, Boolean],
            default: false
        },
        closeOnClickModal: {
            type: Boolean,
            default: true
        },
        modal: {
            type: Boolean,
            default: true
        }
    },
    data() {
        return {
            tabHeight: 40,
            isActive: true,
            visibleVal: this.visible
        };
    },
    computed: {
        headerHeight() {
            return this.$store.state.headerHeight;
        },
        asideWidth() {
            return this.$store.state.asideWidth;
        },
        isMobile() {
            return this.$store.state.isMobile;
        },
        leftPosition() {
            if (this.isMobile) {
                return 0;
            }

            return (
                this.asideWidth + (this._.isNumber(this.left) ? this.left : 0)
            );
        },
        style() {
            const style = {
                top: this.headerHeight + this.tabHeight + 'px',
                bottom: 0
            };
            if (this._.isNumber(this.top)) {
                style.top = this.top + 'px';
            }

            if (this._.isNumber(this.left)) {
                style.width = `calc(100vw - ${this.leftPosition}px)`;
            }

            return style;
        },
        classList() {
            const classList = [];

            if (this.visibleVal) {
                classList.push('show');
            } else {
                classList.push('hide');
            }

            if (this._.isNumber(this.left)) {
                classList.push('width');
            }

            if (this.modal) {
                classList.push('modal');
            }

            return classList;
        }
    },
    methods: {
        handleShadeClick() {
            if (this.closeOnClickModal) {
                this.hide();
            }
        },
        handleSidebarClick() {
            this.hide();
        },
        hide() {
            this.$emit('close');
            this.$emit('update:visible', false);
        },
        showVisibleVal() {
            this.$emit('open');
            this.visibleVal = true;
        }
    },
    watch: {
        visible(val) {
            if (val) {
                if (
                    this.$el &&
                    this.$el.parentNode &&
                    this.$el.parentNode !== document.body
                ) {
                    document.body.appendChild(this.$el);

                    setTimeout(() => {
                        this.showVisibleVal();
                    }, 0);
                } else {
                    this.showVisibleVal();
                }
            } else {
                this.$emit('close');
                this.visibleVal = false;
            }
        }
    },
    mounted() {
        if (this.visibleVal) {
            this.$emit('open');
        }

        document.body.appendChild(this.$el);
    },
    deactivated() {
        this.isActive = false;
    },
    activated() {
        this.isActive = true;
    },
    destroyed() {
        if (this.$el && this.$el.parentNode) {
            this.$el.parentNode.removeChild(this.$el);
        }
    }
};
</script>

<style scoped lang="less">
.ex-drawer {
    /deep/ .el-loading-mask {
        display: none;
    }
    &.el-loading-parent--relative > .ex-drawer__content {
        /deep/ .el-loading-mask {
            display: block;
        }
    }

    &__content {
        @sidebar-color: #e2e6f1;

        position: fixed;
        z-index: 1110;
        right: 0;
        background-color: #fff;
        display: flex;
        background-color: @sidebar-color;
        padding-right: 25px;
        box-sizing: border-box;
        align-items: stretch;
        transition: transform 0.8s;
        transform: translateX(100%);
        max-width: 100vw;

        &.modal {
            box-shadow: none;
        }

        &.show {
            transform: translateX(0);
        }
        &.hide {
            transform: translateX(100%);
        }

        &--sidebar {
            width: 25px;
            text-align: center;
            display: flex;
            justify-content: center;
            align-items: center;
            color: #486dd5;
            cursor: pointer;

            &:hover {
            }
        }
        &--main {
            background-color: #fff;
            box-sizing: border-box;
            padding: 10px;
            // max-width: 1024px - 50px;
            overflow: auto;
        }

        &.width &--main {
            width: 100%;
        }
    }
    &__shade {
        position: fixed;
        top: 0;
        z-index: 2000;
        left: 0;
        bottom: 0;
        right: 0;
        background-color: rgba(0, 0, 0, 0.4);
    }
}
</style>
