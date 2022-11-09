namespace SnkFramework.Mvvm.Runtime
{
    namespace View
    {
        public enum WIN_STATE
        {
            none,
            create_begin,
            create_end,
            
            show_anim_begin,
            visible,
            show_anim_end,
            
            activation_anim_begin,
            activation,
            activation_anim_end,

            passivation_anim_begin,
            passivation,
            passivation_anim_end,
            
            hide_anim_begin,
            invisible,
            hide_anim_end,
            
            destroy_begin,
            destroy_end
        }
    }
}