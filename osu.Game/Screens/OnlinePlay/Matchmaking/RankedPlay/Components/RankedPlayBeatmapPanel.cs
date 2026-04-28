// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Localisation;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Drawables;
using osu.Game.Beatmaps.Drawables.Cards;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Online.API.Requests.Responses;
using osu.Game.Resources.Localisation.Web;
using osu.Game.Rulesets.Mods;
using osu.Game.Screens.Play.HUD;
using osuTK;

namespace osu.Game.Screens.OnlinePlay.Matchmaking.RankedPlay.Components
{
    public partial class RankedPlayBeatmapPanel : CompositeDrawable
    {
        public RankedPlayBeatmapPanel(APIBeatmap apibeatmap, Mod[] mods)
        {
            beatmapSet = apibeatmap.BeatmapSet!;
            beatmap = apibeatmap;
            this.mods = mods;
        }

        private readonly APIBeatmapSet beatmapSet;
        private readonly APIBeatmap beatmap;
        private readonly Mod[] mods;
        private BufferedContainer background = null!;
        private const float corner_radius = 12;
        private const float border_thickness = 3;

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Padding = new MarginPadding
            {
                Left = 30,
                Right = 100,
                Bottom = 10
            };
            InternalChild = new Container
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Children = [
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Child = background = new BufferedContainer(cachedFrameBuffer: true)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Masking = true,
                                CornerRadius = corner_radius,
                                BorderThickness = border_thickness,
                                BorderColour = colours.ForStarDifficulty(beatmap.StarRating).Darken(0.7f),
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Color4Extensions.FromHex("222228"),
                                        Alpha = 0.7f,
                                    }
                                },
                            },
                        }
                    },
                    new FillFlowContainer
                    {
                        Margin = new MarginPadding(10),
                        Spacing = new Vector2(3, 0),
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Children = new Drawable[]
                        {
                        new TruncatingSpriteText
                        {
                            Origin = Anchor.TopLeft,
                            Anchor = Anchor.TopLeft,
                            Text = new RomanisableString(beatmapSet.TitleUnicode, beatmapSet.Title),
                            Font = OsuFont.Default.With(size: 28f, weight: FontWeight.Bold),
                        },
                        new GridContainer
                        {
                            AutoSizeAxes = Axes.Both,
                            ColumnDimensions = new[]
                            {
                                new Dimension(GridSizeMode.AutoSize),
                                new Dimension(GridSizeMode.AutoSize)
                            },
                            RowDimensions = new[]
                            {
                                new Dimension(GridSizeMode.AutoSize)
                            },
                            Content = new[]
                            {
                                new Drawable[]
                                {
                                    new TruncatingSpriteText
                                    {
                                        Origin = Anchor.CentreLeft,
                                        Anchor = Anchor.CentreLeft,
                                        Text = BeatmapsetsStrings.ShowDetailsByArtist(new RomanisableString(beatmapSet.ArtistUnicode, beatmapSet.Artist)),
                                        Font = OsuFont.Default.With(size: 18f, weight: FontWeight.SemiBold),
                                    },
                                    new LinkFlowContainer(s =>
                                        {
                                            s.Shadow = false;
                                            s.Font = OsuFont.Default.With(size: 18f, weight: FontWeight.SemiBold);
                                            Origin = Anchor.CentreLeft;
                                            Anchor = Anchor.CentreLeft;

                                        }
                                    ).With(d =>
                                        {
                                            d.AddText("     mapped by ", t => t.Colour = colours.Blue);
                                            d.AddUserLink(beatmapSet.Author);
                                        }
                                    )
                                },
                            }
                        },
                        new GridContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Origin = Anchor.TopLeft,
                                Anchor = Anchor.TopLeft,
                                Padding = new MarginPadding
                                {
                                    Top = 5
                                },
                                ColumnDimensions = new[]
                                {
                                    new Dimension(),
                                    new Dimension(GridSizeMode.AutoSize)
                                },
                                RowDimensions = new[]
                                {
                                    new Dimension(GridSizeMode.AutoSize)
                                },
                                Content = new[]
                                {
                                    new Drawable[]
                                    {
                                        new Container
                                        {
                                            Masking = true,
                                            CornerRadius = BeatmapCard.CORNER_RADIUS,
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y,
                                            Children = new Drawable[]
                                            {
                                                new Box
                                                {
                                                    Colour = colours.ForStarDifficulty(beatmap.StarRating).Darken(0.8f),
                                                    RelativeSizeAxes = Axes.Both,
                                                },
                                                new FillFlowContainer
                                                {
                                                    Padding = new MarginPadding(4),
                                                    RelativeSizeAxes = Axes.X,
                                                    AutoSizeAxes = Axes.Y,
                                                    Direction = FillDirection.Horizontal,
                                                    Spacing = new Vector2(6, 0),
                                                    Children = new Drawable[]
                                                    {
                                                        new StarRatingDisplay(new StarDifficulty(beatmap.StarRating, 0), StarRatingDisplaySize.Small, animated: true)
                                                        {
                                                            Origin = Anchor.CentreLeft,
                                                            Anchor = Anchor.CentreLeft,
                                                            Scale = new Vector2(0.9f),
                                                        },
                                                        new TruncatingSpriteText
                                                        {
                                                            Text = beatmap.DifficultyName,
                                                            Font = OsuFont.Style.Caption1.With(weight: FontWeight.Bold),
                                                            Colour = colours.ForStarDifficultyText(beatmap.StarRating),
                                                            Anchor = Anchor.CentreLeft,
                                                            Origin = Anchor.CentreLeft,
                                                        },
                                                    }
                                                },
                                            }
                                        },
                                        new Container
                                        {
                                            AutoSizeAxes = Axes.Both,
                                            Alpha = mods.Length > 0 ? 1 : 0,
                                            Child = new ModFlowDisplay
                                            {
                                                AutoSizeAxes = Axes.Both,
                                                Scale = new Vector2(0.5f),
                                                Margin = new MarginPadding { Left = 5 },
                                                Current = { Value = mods },
                                            }
                                        }
                                    },
                                }
                            },
                        }
                    },
                ]
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            FinishTransforms(true);
        }
    }
}
